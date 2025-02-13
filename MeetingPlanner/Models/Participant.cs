using System;
using System.Collections.Generic;

namespace MeetingPlanner.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public List<ParticipantMeeting> ParticipantMeetings { get; set; } = new();
    }
}
