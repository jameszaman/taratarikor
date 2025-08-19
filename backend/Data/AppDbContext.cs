using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Backend.Entities;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TicketAssignee> TicketAssignees => Set<TicketAssignee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.UseSerialColumns();

            // TicketAssignee composite key
            modelBuilder.Entity<TicketAssignee>()
                .HasKey(ta => new { ta.TicketId, ta.UserId });

            // Relationships
            modelBuilder.Entity<TicketAssignee>()
                .HasOne(ta => ta.Ticket)
                .WithMany(t => t.Assignments)
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketAssignee>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.TicketAssignments)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
