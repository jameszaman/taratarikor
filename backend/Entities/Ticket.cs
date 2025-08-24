using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace Backend.Entities
{
    public enum TicketStatus { Open = 0, InProgress = 1, Resolved = 2, Closed = 3 }
    public enum TicketPriority { Low = 0, Medium = 1, High = 2 }

    [Index(nameof(Status))]
    [Index(nameof(Priority))]
    [Index(nameof(Title))]
    [Index(nameof(Status), nameof(Priority), nameof(CreatedAt))]
    public class Ticket
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Low;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<TicketAssignee> Assignments { get; set; } = new List<TicketAssignee>();

        public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();

        // This will be used to search using either title or description.
        public NpgsqlTsVector? SearchVector { get; set; }

        [Timestamp]
        public uint Xmin { get; set; }
    }
}
