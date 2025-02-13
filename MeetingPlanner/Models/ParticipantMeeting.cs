using System;

namespace MeetingPlanner.Models
{
    public class ParticipantMeeting
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int ParticipantId { get; set; }

        public Meeting? Meeting { get; set; }
        public Participant? Participant { get; set; }
    }
}
