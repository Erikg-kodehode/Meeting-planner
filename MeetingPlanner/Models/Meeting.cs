using System;
using System.Collections.Generic;

namespace MeetingPlanner.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public List<string> Participants { get; set; } = new List<string>();
    }
}
