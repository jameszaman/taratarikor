using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Tag
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
        public ICollection<UserTag> UserTags { get; set; } = new List<UserTag>();
    }
}
