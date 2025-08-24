using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(254)]
        public string Email { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        public ICollection<TicketAssignee> TicketAssignments { get; set; } = new List<TicketAssignee>();

        public ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();

        [Timestamp]
        public uint Xmin { get; set; }
    }
}
