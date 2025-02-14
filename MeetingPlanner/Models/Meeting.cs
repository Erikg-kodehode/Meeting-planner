using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingPlanner.Models
{
    public class Meeting
    {
        [Key] // ✅ Sets Id as the Primary Key
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable for infinite meetings
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = "Ukjent";
        public List<string> Participants { get; set; } = new List<string>();
    }
}
