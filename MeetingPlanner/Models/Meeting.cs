using System;
using System.Collections.Generic;

namespace MeetingPlanner.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Tittel { get; set; } = string.Empty;
        public DateTime StartTid { get; set; }
        public DateTime SluttTid { get; set; }
        public string Sted { get; set; } = string.Empty;
        public string Beskrivelse { get; set; } = string.Empty;

        public string OpprettetAv { get; set; } = "Ukjent";
        public DateTime OpprettetTidspunkt { get; private set; }

        // Liste over deltakere
        public List<string> Deltakere { get; set; } = new();

        public Meeting()
        {
            OpprettetTidspunkt = DateTime.UtcNow;
        }
    }
}
