using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public enum AssigneeRole { Assignee = 0, Reviewer = 1, Approver = 2 }

    public class TicketAssignee
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }

        public Ticket Ticket { get; set; } = default!;
        public User User { get; set; } = default!;

        public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;

        public AssigneeRole Role { get; set; } = AssigneeRole.Assignee;
    }
}
