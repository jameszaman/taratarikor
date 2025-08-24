namespace Backend.Entities
{
    public class UserTag
    {
        public int UserId { get; set; }
        public int TagId { get; set; }

        public User User { get; set; } = default!;
        public Tag Tag { get; set; } = default!;
    }
}
