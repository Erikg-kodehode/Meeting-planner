using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public class MeetingStorageService
    {
        private readonly string logFile = "møter.json";

        public List<Meeting> LoadMeetings()
        {
            try
            {
                if (File.Exists(logFile))
                {
                    string jsonData = File.ReadAllText(logFile);
                    List<Meeting> storedMeetings = JsonSerializer.Deserialize<List<Meeting>>(jsonData) ?? new List<Meeting>();

                    Console.WriteLine("\n🔍 Laster inn lagrede møter:");
                    foreach (var meeting in storedMeetings)
                    {
                        Console.WriteLine($"[{meeting.Id}] {meeting.Title} med {meeting.Participants.Count} deltakere");
                    }
                    return storedMeetings;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Feil ved lesing av fil: {e.Message}");
            }

            return new List<Meeting>();
        }

        public void SaveMeetings(List<Meeting> meetings)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(logFile, jsonData);

                Console.WriteLine("\n📅 ✅ Lagt til i Kalender.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Feil ved skriving til fil: {e.Message}");
            }
        }
    }
}
