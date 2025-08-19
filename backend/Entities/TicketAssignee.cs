using System;

namespace Backend.Entities
{
    public class TicketAssignee
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }

        // Navigation
        public Ticket Ticket { get; set; } = default!;
        public User User { get; set; } = default!;

        public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;
        public string Role { get; set; } = "Assignee"; // e.g., Assignee, Reviewer, Approver
    }
}
