using System;
using System.Collections.Generic;
using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public class MeetingService
    {
        private readonly List<Meeting> meetings;
        private readonly MeetingStorageService storageService;
        private readonly MeetingInputService inputService;

        public MeetingService()
        {
            storageService = new MeetingStorageService();
            inputService = new MeetingInputService();
            meetings = storageService.LoadMeetings();
        }

        public void AddMeeting()
        {
            Console.WriteLine("\n📅 Nytt møte");

            Console.Write("Tittel: ");
            string title = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Sted: ");
            string location = Console.ReadLine()?.Trim() ?? "";

            DateTime startTime = inputService.GetTimeAndDate("Starttid (HH:mm eller 4 siffer, f.eks. 1400): ", "Startdato (dd.MM.yyyy eller 'i dag'): ");

            DateTime? endTime = inputService.GetTimeOrInfinite("Slutttid (HH:mm eller 4 siffer, f.eks. 1500, eller blank for uendelig): ", "Sluttdato (dd.MM.yyyy eller 'i dag'): ", startTime);

            Console.Write("Hvem oppretter møtet? ");
            string createdBy = Console.ReadLine()?.Trim() ?? "Ukjent";

            Console.Write("Kort beskrivelse: ");
            string description = Console.ReadLine()?.Trim() ?? "";

            List<string> participants = new List<string>();
            Console.WriteLine("Skriv inn deltakere (trykk Enter for å stoppe): ");
            while (true)
            {
                Console.Write("Deltaker: ");
                Console.Write("Deltaker: ");
                string participant = Console.ReadLine()?.Trim() ?? ""; // Ensure it's never null

                if (string.IsNullOrWhiteSpace(participant))
                    break;

                participants.Add(participant);

                if (string.IsNullOrWhiteSpace(participant)) break;
                participants.Add(participant);
            }

            Meeting meeting = new Meeting
            {
                Id = meetings.Count + 1,
                Title = title,
                StartTime = startTime,
                EndTime = endTime,
                Location = location,
                Description = description,
                CreatedBy = createdBy,
                Participants = participants
            };

            meetings.Add(meeting);
            storageService.SaveMeetings(meetings);
            Console.WriteLine($"\n✅ Møte '{title}' lagt til!\n");
        }

        public void DeleteMeeting()
        {
            if (meetings.Count == 0)
            {
                Console.WriteLine("\n❌ Ingen møter å slette.");
                return;
            }

            Console.WriteLine("\n📅 Velg et møte å slette:");
            foreach (Meeting meeting in meetings)
            {
                string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("dd.MM.yyyy HH:mm") : "Uendelig";
                Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:dd.MM.yyyy HH:mm} til {endTime}");
            }

            Console.Write("\nSkriv ID på møtet du vil slette: ");
            if (int.TryParse(Console.ReadLine(), out int meetingId))
            {
                Meeting meeting = meetings.Find(m => m.Id == meetingId);
                if (meeting != null)
                {
                    Console.Write($"❗ Er du sikker på at du vil slette møtet '{meeting.Title}'? (ja / nei): ");
                    string confirmation = Console.ReadLine()?.Trim().ToLower() ?? "";

                    if (confirmation == "ja")
                    {
                        meetings.Remove(meeting);
                        storageService.SaveMeetings(meetings);
                        Console.WriteLine($"✅ Møte '{meeting.Title}' er slettet.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Sletting avbrutt.");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                }
            }
        }

        public void DisplayMeetings()
        {
            if (meetings.Count == 0)
            {
                Console.WriteLine("\n❌ Ingen møter planlagt.");
                return;
            }

            Console.WriteLine("\n📅 Planlagte møter:");
            foreach (Meeting meeting in meetings)
            {
                string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("dd.MM.yyyy HH:mm") : "Uendelig";
                Console.WriteLine($"- [{meeting.Id}] {meeting.Title} på {meeting.Location} ({meeting.StartTime:dd.MM.yyyy HH:mm} - {endTime})");
            }
        }
    }
}
