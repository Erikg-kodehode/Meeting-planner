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

            Console.Write("⏰ Starttid (HH:mm eller blank for nå +5 min): ");
            var startTimeInput = Console.ReadLine()?.Trim();
            var startTime = string.IsNullOrWhiteSpace(startTimeInput) ? DateTime.Now.AddMinutes(5) : ParseTimeInput(startTimeInput);

            Console.Write("🕘 Slutttid (HH:mm eller blank for uendelig): ");
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
                var endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Uendelig";
                Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:HH:mm} til {endTime}");
            }
        }

        public void ViewMeetingDetails()
        {
            using var dbContext = new MeetingDbContext();
            var meetings = dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen planlagte møter.");
                return;
            }

            Console.Write("\nSkriv inn ID for møtet du vil se detaljer for: ");
            if (int.TryParse(Console.ReadLine(), out var meetingId))
            {
                var meeting = dbContext.Meetings.Find(meetingId);
                if (meeting != null)
                {
                    Console.WriteLine("\n📋 Møtedetaljer:");
                    Console.WriteLine($"📌 Tittel: {meeting.Title}");
                    Console.WriteLine($"📍 Sted: {meeting.Location}");
                    Console.WriteLine($"⏰ Starttid: {meeting.StartTime:HH:mm}");
                    Console.WriteLine($"🕘 Slutttid: {(meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Uendelig")}");
                    Console.WriteLine($"👤 Opprettet av: {meeting.CreatedBy}");
                    Console.WriteLine($"📝 Beskrivelse: {meeting.Description}");

                    Console.WriteLine("👥 Deltakere:");
                    if (meeting.Participants.Any())
                    {
                        foreach (var participant in meeting.Participants)
                        {
                            Console.WriteLine($"- {participant}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ingen deltakere registrert.");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                }
            }
            else
            {
                Console.WriteLine("❌ Ugyldig ID. Prøv igjen.");
            }
        }

        public void DeleteMeeting()
        {
            using var dbContext = new MeetingDbContext();
            var meetings = dbContext.Meetings.ToList();

            if (!meetings.Any())
            {
                Console.WriteLine("\n❌ Ingen møter å slette.");
                return;
            }

            Console.Write("\nSkriv ID på møtet du vil slette: ");
            if (int.TryParse(Console.ReadLine(), out var meetingId))
            {
                var meeting = dbContext.Meetings.Find(meetingId);
                if (meeting != null)
                {
                    dbContext.Meetings.Remove(meeting);
                    dbContext.SaveChanges();
                    Console.WriteLine($"✅ Møte '{meeting.Title}' er slettet fra databasen.");
                }
                else
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                }
            }
            else
            {
                Console.WriteLine("❌ Ugyldig ID. Prøv igjen.");
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

                while (true)
                {
                    Console.WriteLine("\n🔹 Hva vil du redigere?");
                    Console.WriteLine("1. Tittel");
                    Console.WriteLine("2. Sted");
                    Console.WriteLine("3. Starttid");
                    Console.WriteLine("4. Slutttid");
                    Console.WriteLine("5. Beskrivelse");
                    Console.WriteLine("6. Administrer deltakere");
                    Console.WriteLine("7. Lagre og avslutt");
                    Console.Write("Velg et alternativ: ");

                    var choice = Console.ReadLine()?.Trim();
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

                        case "6":
                            ManageParticipants(meeting);
                            break;

                        case "7":
                            dbContext.SaveChanges();
                            Console.WriteLine("\n✅ Endringer lagret!");
                            return;

                        default:
                            Console.WriteLine("❌ Ugyldig valg. Prøv igjen.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("❌ Ugyldig ID. Prøv igjen.");
            }
        }
        private void ManageParticipants(Meeting meeting)
        {
            while (true)
            {
                Console.WriteLine("\n👥 Deltakerhåndtering:");
                Console.WriteLine("1. ➕ Legg til en deltaker");
                Console.WriteLine("2. ❌ Fjern en deltaker");
                Console.WriteLine("3. 🔙 Gå tilbake");
                Console.Write("👉 Velg et alternativ: ");

                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": // Add participant
                        Console.Write("✏️ Skriv inn navnet på deltakeren som skal legges til: ");
                        var newParticipant = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrWhiteSpace(newParticipant) && !meeting.Participants.Contains(newParticipant))
                        {
                            meeting.Participants.Add(newParticipant);
                            Console.WriteLine($"✅ {newParticipant} er lagt til!");
                        }
                        else
                        {
                            Console.WriteLine("❌ Ugyldig eller eksisterende deltaker.");
                        }
                        break;

                    case "2": // Remove participant
                        if (!meeting.Participants.Any())
                        {
                            Console.WriteLine("❌ Ingen deltakere å fjerne.");
                            break;
                        }

                        Console.WriteLine("\n🔻 Velg en deltaker å fjerne:");
                        for (int i = 0; i < meeting.Participants.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {meeting.Participants[i]}");
                        }

                        Console.Write("Skriv inn nummeret på deltakeren du vil fjerne: ");
                        if (int.TryParse(Console.ReadLine(), out var index) && index > 0 && index <= meeting.Participants.Count)
                        {
                            Console.WriteLine($"❌ {meeting.Participants[index - 1]} er fjernet.");
                            meeting.Participants.RemoveAt(index - 1);
                        }
                        else
                        {
                            Console.WriteLine("❌ Ugyldig valg.");
                        }
                        break;

                    case "3": // Exit participant management
                        return;

                    default:
                        Console.WriteLine("❌ Ugyldig valg. Prøv igjen.");
                        break;
                }
            }
        }



        private DateTime ParseTimeInput(string timeInput)
        {
            return string.IsNullOrWhiteSpace(timeInput) ? DateTime.Now.AddMinutes(5) :
                   DateTime.TryParseExact(timeInput, "HH:mm", null, DateTimeStyles.None, out var parsedTime) ? parsedTime :
                   DateTime.Now.AddMinutes(5);
        }
    }
}
