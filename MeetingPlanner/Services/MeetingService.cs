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
            using (var dbContext = new MeetingDbContext()) // Ensure database context
            {
                Console.Write("\n📌 Skriv inn tittel for møtet: ");
                string title = Console.ReadLine()?.Trim() ?? "Uten tittel";

                Console.Write("📍 Sted: ");
                string location = Console.ReadLine()?.Trim() ?? "Ikke spesifisert";

                Console.Write("⏰ Starttid (HH:mm eller 4 siffer, f.eks. 1400, eller blank for nå +5 min): ");
                string startTimeInput = Console.ReadLine()?.Trim();
                DateTime startTime = ParseTimeInput(startTimeInput);

                Console.Write("🕘 Slutttid (HH:mm eller 4 siffer, blank for uendelig): ");
                string endTimeInput = Console.ReadLine()?.Trim();
                DateTime? endTime = string.IsNullOrWhiteSpace(endTimeInput) ? null : ParseTimeInput(endTimeInput);

                Console.Write("👤 Hvem oppretter møtet? ");
                string createdBy = Console.ReadLine()?.Trim() ?? "Ukjent";

                Console.Write("📝 Kort beskrivelse: ");
                string description = Console.ReadLine()?.Trim() ?? "Ingen beskrivelse";

                List<string> participants = new List<string>();
                Console.WriteLine("👥 Skriv inn deltakere (trykk Enter for å stoppe):");
                while (true)
                {
                    Console.Write("Deltaker: ");
                    string participant = Console.ReadLine()?.Trim();
                    if (string.IsNullOrWhiteSpace(participant)) break;
                    participants.Add(participant);
                }

                Meeting newMeeting = new Meeting
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
        }

        public void DisplayMeetings()
        {
            using (var dbContext = new MeetingDbContext())
            {
                var meetings = dbContext.Meetings.ToList();

                if (!meetings.Any())
                {
                    Console.WriteLine("\n❌ Ingen planlagte møter.");
                    return;
                }

                Console.WriteLine("\n📅 Planlagte møter:");
                foreach (var meeting in meetings)
                {
                    string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Ikke angitt";
                    Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:HH:mm} til {endTime}");
                }
            }
        }

        public void ViewMeetingDetails()
        {
            using (var dbContext = new MeetingDbContext()) // Ensures a fresh database context
            {
                var meetings = dbContext.Meetings.ToList();

                if (!meetings.Any())
                {
                    Console.WriteLine("\n❌ Ingen planlagte møter.");
                    return;
                }

                Console.WriteLine("\n📅 Velg et møte for å se detaljer:");
                foreach (var meeting in meetings)
                {
                    string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Uendelig";
                    Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:HH:mm} til {endTime}");
                }

                Console.Write("\nSkriv inn ID for møtet du vil se detaljer for: ");
                if (int.TryParse(Console.ReadLine(), out int meetingId))
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
        }

        public void DeleteMeeting()
        {
            using (var dbContext = new MeetingDbContext()) // Ensure fresh context
            {
                var meetings = dbContext.Meetings.ToList();

                if (!meetings.Any())
                {
                    Console.WriteLine("\n❌ Ingen møter å slette.");
                    return;
                }

                Console.WriteLine("\n📅 Velg et møte å slette:");
                foreach (var meeting in meetings)
                {
                    string endTime = meeting.EndTime.HasValue ? meeting.EndTime.Value.ToString("HH:mm") : "Ikke angitt";
                    Console.WriteLine($"[{meeting.Id}] {meeting.Title} - {meeting.StartTime:HH:mm} til {endTime}");
                }

                Console.Write("\nSkriv ID på møtet du vil slette: ");
                if (int.TryParse(Console.ReadLine(), out int meetingId))
                {
                    var meeting = dbContext.Meetings.Find(meetingId);
                    if (meeting != null)
                    {
                        try
                        {
                            dbContext.Meetings.Remove(meeting);
                            dbContext.SaveChanges(); // Commit deletion

                            // 🔥 Reset Auto-Increment
                            dbContext.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='Meetings';");

                            Console.WriteLine($"✅ Møte '{meeting.Title}' er slettet fra databasen.");
                        }
                        catch (Microsoft.Data.Sqlite.SqliteException ex)
                        {
                            if (ex.SqliteErrorCode == 5) // SQLITE_BUSY (Database is locked)
                            {
                                Console.WriteLine("❌ Database er låst. Lukk eventuelle andre programmer som bruker databasen og prøv igjen.");
                            }
                            else
                            {
                                Console.WriteLine($"❌ Feil ved sletting: {ex.Message}");
                            }
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

            if (DateTime.TryParseExact(timeInput, "HH:mm", null, DateTimeStyles.None, out DateTime parsedTime))
            {
                return parsedTime;
            }

            Console.WriteLine("❌ Ugyldig tidsformat! Bruk 'HH:mm' eller 4 siffer (f.eks. 1400).");
            return ParseTimeInput(Console.ReadLine()?.Trim() ?? "");
        }
    }
}
