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
            if (File.Exists(logFile))
            {
                string jsonData = File.ReadAllText(logFile);
                return JsonSerializer.Deserialize<List<Meeting>>(jsonData) ?? new List<Meeting>();
            }
            return new List<Meeting>();
        }

        public void SaveMeetings(List<Meeting> meetings)
        {
            File.WriteAllText(logFile, JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
