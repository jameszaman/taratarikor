using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    public enum TicketStatus { Open = 0, InProgress = 1, Resolved = 2, Closed = 3 }
    public enum TicketPriority { Low = 0, Medium = 1, High = 2 }

    [Index(nameof(Status))]
    [Index(nameof(Priority))]
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<TicketAssignee> Assignments { get; set; } = new List<TicketAssignee>();

        public TicketPriority Priority { get; set; } = TicketPriority.Low;

        public List<string> Tags { get; set; } = new();
    }
}
