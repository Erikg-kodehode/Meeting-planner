using System;
using System.Collections.Generic;
using System.Linq;
using MeetingPlanner.Models;
using MeetingPlanner.Services;

namespace MeetingPlanner.Services
{
    public class MeetingService
    {
        private readonly MeetingDbContext _dbContext;
        private readonly MeetingInputService _inputService;

        public MeetingService()
        {
            _dbContext = new MeetingDbContext();
            _inputService = new MeetingInputService();
            _dbContext.Database.EnsureCreated(); // ✅ Creates DB if it doesn't exist
        }

        public void AddMeeting()
        {
            Console.WriteLine("\n📅 Nytt møte");

            Console.Write("Tittel: ");
            string title = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Sted: ");
            string location = Console.ReadLine()?.Trim() ?? "";

            DateTime startTime = _inputService.GetTimeAndDate(
                "Starttid (HH:mm eller 4 siffer, f.eks. 1400, eller blank for nå +5 min): ",
                "Startdato (dd.MM.yyyy eller 'i dag'): "
            );

            DateTime? endTime = _inputService.GetTimeOrInfinite(
                "Slutttid (HH:mm eller 4 siffer, f.eks. 1500, eller blank for uendelig): ",
                "Sluttdato (dd.MM.yyyy eller 'i dag'): ",
                startTime
            );

            Console.Write("Hvem oppretter møtet? ");
            string createdBy = Console.ReadLine()?.Trim() ?? "Ukjent";

            Console.Write("Kort beskrivelse: ");
            string description = Console.ReadLine()?.Trim() ?? "";

            List<string> participants = new List<string>();
            Console.WriteLine("Skriv inn deltakere (trykk Enter for å stoppe): ");
            while (true)
            {
                Console.Write("Deltaker: ");
                string participant = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(participant)) break;
                participants.Add(participant);
            }

            Meeting meeting = new Meeting
            {
                Title = title,
                StartTime = startTime,
                EndTime = endTime,
                Location = location,
                Description = description,
                CreatedBy = createdBy,
                Participants = participants
            };

            _dbContext.Meetings.Add(meeting);
            _dbContext.SaveChanges();
            Console.WriteLine($"\n✅ Møte '{title}' lagt til i kalenderen!\n");
        }

        public void DisplayMeetings()
        {
            var meetings = _dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen møter planlagt.");
                return;
            }

            Console.WriteLine("\n📅 Planlagte møter:");
            foreach (var meeting in meetings)
            {
                string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("dd.MM.yyyy HH:mm") : "Uendelig";
                Console.WriteLine($"- [{meeting.Id}] {meeting.Title} på {meeting.Location} ({meeting.StartTime:dd.MM.yyyy HH:mm} - {endTime})");
            }
        }

        public void DeleteMeeting()
        {
            var meetings = _dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen møter å slette.");
                return;
            }

            Console.WriteLine("\n📅 Velg et møte å slette:");
            foreach (var meeting in meetings)
            {
                string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("dd.MM.yyyy HH:mm") : "Uendelig";
                Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:dd.MM.yyyy HH:mm} til {endTime}");
            }

            Console.Write("\nSkriv ID på møtet du vil slette: ");
            if (int.TryParse(Console.ReadLine(), out int meetingId))
            {
                var meeting = _dbContext.Meetings.Find(meetingId);
                if (meeting != null)
                {
                    _dbContext.Meetings.Remove(meeting);
                    _dbContext.SaveChanges();
                    Console.WriteLine($"✅ Møte '{meeting.Title}' er slettet.");
                }
                else
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                }
            }
        }
    }
}
