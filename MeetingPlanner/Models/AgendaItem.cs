using System;

namespace MeetingPlanner.Models
{
    public class AgendaItem
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }

        public Meeting? Meeting { get; set; }
    }
}
