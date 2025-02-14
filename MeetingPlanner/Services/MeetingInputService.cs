using System;
using System.Globalization;

namespace MeetingPlanner.Services
{
    public class MeetingInputService
    {
        public DateTime GetTimeAndDate(string timeMessage, string dateMessage, DateTime? afterDate = null)
        {
            Console.Write(timeMessage);
            string timeInput = Console.ReadLine()?.Trim() ?? "";
            timeInput = timeInput.Length == 4 ? timeInput.Insert(2, ":") : timeInput;
            TimeSpan time = TimeSpan.Parse(timeInput);

            Console.Write(dateMessage);
            string dateInput = Console.ReadLine()?.Trim() ?? "";
            DateTime date = dateInput == "i dag" ? DateTime.Today : DateTime.ParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            return date.Add(time);
        }

        public DateTime? GetTimeOrInfinite(string timeMessage, string dateMessage, DateTime afterDate)
        {
            Console.Write(timeMessage);
            string timeInput = Console.ReadLine()?.Trim() ?? "";
            return string.IsNullOrWhiteSpace(timeInput) ? (DateTime?)null : GetTimeAndDate(timeMessage, dateMessage, afterDate);
        }
    }
}
