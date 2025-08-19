using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Unique + indexed for quick lookup
        public string Email { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation: all ticket assignments for this user
        public ICollection<TicketAssignee> TicketAssignments { get; set; } = new List<TicketAssignee>();
    }
}
