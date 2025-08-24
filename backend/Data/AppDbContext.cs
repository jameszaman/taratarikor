using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using NpgsqlTypes;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TicketAssignee> TicketAssignees => Set<TicketAssignee>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TicketTag> TicketTags => Set<TicketTag>();
        public DbSet<UserTag> UserTags => Set<UserTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");
            modelBuilder.UseSerialColumns();

            // Enable needed Postgres extensions
            modelBuilder.HasPostgresExtension("citext");     // case-insensitive text
            modelBuilder.HasPostgresExtension("pg_trgm");    // optional: trigram search speedups

            // TicketAssignee composite key + relationships
            modelBuilder.Entity<TicketAssignee>(e =>
            {
                e.HasKey(x => new { x.TicketId, x.UserId });

                e.HasOne(x => x.Ticket)
                 .WithMany(t => t.Assignments)
                 .HasForeignKey(x => x.TicketId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                 .WithMany(u => u.TicketAssignments)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(e =>
            {
                e.Property(t => t.Title).HasMaxLength(200).IsRequired();

                // Generated tsvector over title + description
                e.HasGeneratedTsVectorColumn(
                        t => t.SearchVector!,
                        "english",
                        t => new { t.Title, t.Description })
                 .HasIndex(t => t.SearchVector)
                 .HasMethod("GIN");
            });

            // Tag: Name as CITEXT + unique
            modelBuilder.Entity<Tag>(e =>
            {
                e.Property(x => x.Name).HasColumnType("citext").HasMaxLength(64).IsRequired();
                e.HasIndex(x => x.Name).IsUnique();
            });

            // TicketTag M2M
            modelBuilder.Entity<TicketTag>(e =>
            {
                e.HasKey(x => new { x.TicketId, x.TagId });
                e.HasOne(x => x.Ticket)
                 .WithMany(t => t.TicketTags)
                 .HasForeignKey(x => x.TicketId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Tag)
                 .WithMany(t => t.TicketTags)
                 .HasForeignKey(x => x.TagId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // UserTag M2M
            modelBuilder.Entity<UserTag>(e =>
            {
                e.HasKey(x => new { x.UserId, x.TagId });
                e.HasOne(x => x.User)
                 .WithMany(u => u.UserTags)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Tag)
                 .WithMany(t => t.UserTags)
                 .HasForeignKey(x => x.TagId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Xmin)
                .IsRowVersion();   // maps to xmin

            modelBuilder.Entity<User>()
                .Property(u => u.Xmin)
                .IsRowVersion();   // maps to xmin
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var e in ChangeTracker.Entries())
            {
                if (e.State == EntityState.Modified && e.Metadata.FindProperty("UpdatedAt") != null)
                    e.CurrentValues["UpdatedAt"] = now;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
