using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MeetingPlanner.Models;
using System.Globalization;

namespace MeetingPlanner.Services
{
    public class MeetingService
    {
        public void AddMeeting()
        {
            using var dbContext = new MeetingDbContext();

            Console.Write("\n📌 Skriv inn tittel for møtet: ");
            var title = Console.ReadLine()?.Trim() ?? "Uten tittel";

            Console.Write("📍 Sted: ");
            var location = Console.ReadLine()?.Trim() ?? "Ikke spesifisert";

            Console.Write("⏰ Starttid (HH:mm eller 4 siffer, blank for nå +5 min): ");
            var startTimeInput = Console.ReadLine()?.Trim();
            var startTime = ParseTimeInput(startTimeInput);

            Console.Write("🕘 Slutttid (HH:mm eller 4 siffer, blank for uendelig): ");
            var endTimeInput = Console.ReadLine()?.Trim();
            var endTime = string.IsNullOrWhiteSpace(endTimeInput) ? (DateTime?)null : ParseTimeInput(endTimeInput);

            Console.Write("👤 Hvem oppretter møtet? ");
            var createdBy = Console.ReadLine()?.Trim() ?? "Ukjent";

            Console.Write("📝 Kort beskrivelse: ");
            var description = Console.ReadLine()?.Trim() ?? "Ingen beskrivelse";

            var participants = new List<string>();
            Console.WriteLine("👥 Skriv inn deltakere (trykk Enter for å stoppe):");
            while (true)
            {
                Console.Write("Deltaker: ");
                var participant = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(participant)) break;
                participants.Add(participant);
            }

            var newMeeting = new Meeting
            {
                Title = title,
                Location = location,
                StartTime = startTime,
                EndTime = endTime,
                CreatedBy = createdBy,
                Description = description,
                Participants = participants
            };

            dbContext.Meetings.Add(newMeeting);
            dbContext.SaveChanges();
            Console.WriteLine("\n✅ 📅 Møte lagt til kalenderen!");
        }

        public void DisplayMeetings()
        {
            using var dbContext = new MeetingDbContext();
            var meetings = dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen planlagte møter.");
                return;
            }

            Console.WriteLine("\n📅 Planlagte møter:");
            foreach (var meeting in meetings)
            {
                var endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Ikke angitt";
                Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:HH:mm} til {endTime}");
            }
        }

        public void EditMeeting()
        {
            using var dbContext = new MeetingDbContext();
            var meetings = dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen planlagte møter å redigere.");
                return;
            }

            Console.Write("\nSkriv inn ID for møtet du vil redigere: ");
            if (int.TryParse(Console.ReadLine(), out int meetingId))
            {
                var meeting = dbContext.Meetings.Find(meetingId);
                if (meeting == null)
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                    return;
                }

                Console.WriteLine("\nHvilken informasjon vil du redigere?");
                Console.WriteLine("1. Tittel");
                Console.WriteLine("2. Sted");
                Console.WriteLine("3. Starttid");
                Console.WriteLine("4. Slutttid");
                Console.WriteLine("5. Beskrivelse");
                Console.WriteLine("6. Administrer deltakere");
                Console.Write("Velg et alternativ: ");

                string choice = Console.ReadLine()?.Trim();
                switch (choice)
                {
                    case "1":
                        Console.Write("✏️ Ny tittel: ");
                        var newTitle = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(newTitle)) meeting.Title = newTitle;
                        break;

                    case "2":
                        Console.Write("📍 Nytt sted: ");
                        var newLocation = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(newLocation)) meeting.Location = newLocation;
                        break;

                    case "3":
                        Console.Write("⏰ Ny starttid (HH:mm eller blank for nå +5 min): ");
                        var newStartTime = Console.ReadLine()?.Trim();
                        meeting.StartTime = string.IsNullOrWhiteSpace(newStartTime) ? DateTime.Now.AddMinutes(5) : ParseTimeInput(newStartTime);
                        break;

                    case "4":
                        Console.Write("🕘 Ny slutttid (HH:mm eller blank for uendelig): ");
                        var newEndTime = Console.ReadLine()?.Trim();
                        meeting.EndTime = string.IsNullOrWhiteSpace(newEndTime) ? (DateTime?)null : ParseTimeInput(newEndTime);
                        break;

                    case "5":
                        Console.Write("📝 Ny beskrivelse: ");
                        var newDescription = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(newDescription)) meeting.Description = newDescription;
                        break;

                    default:
                        Console.WriteLine("❌ Ugyldig valg. Prøv igjen.");
                        return;
                }

                dbContext.SaveChanges();
                Console.WriteLine("✅ Møteoppdatering lagret!");
            }
            else
            {
                Console.WriteLine("❌ Ugyldig ID. Prøv igjen.");
            }
        }

        private DateTime ParseTimeInput(string timeInput)
        {
            if (string.IsNullOrWhiteSpace(timeInput))
            {
                return DateTime.Now.AddMinutes(5);
            }

            if (timeInput.Length == 4 && int.TryParse(timeInput, out _))
            {
                timeInput = timeInput.Insert(2, ":");
            }

            return DateTime.TryParseExact(timeInput, "HH:mm", null, DateTimeStyles.None, out var parsedTime) ? parsedTime : DateTime.Now.AddMinutes(5);
        }
    }
}
