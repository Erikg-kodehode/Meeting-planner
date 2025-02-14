using System;
using System.Collections.Generic;

namespace MeetingPlanner.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable for infinite meetings
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = "Ukjent";
        public DateTime CreatedAt { get; private set; }
        public List<string> Participants { get; set; } = new List<string>();

        public Meeting()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
