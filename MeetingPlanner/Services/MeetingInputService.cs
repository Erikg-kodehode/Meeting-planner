using System;
using System.Globalization;

namespace MeetingPlanner.Services
{
    public class MeetingInputService
    {
        public DateTime GetTimeAndDate(string timeMessage, string dateMessage)
        {
            DateTime now = DateTime.Now;
            DateTime selectedTime;
            string timeInput, dateInput;

            // ✅ Get Time Input
            while (true)
            {
                Console.Write(timeMessage);
                timeInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(timeInput))
                {
                    selectedTime = now.AddMinutes(5); // ✅ Default to current time + 5 min
                    Console.WriteLine($"⏳ Ingen tid oppgitt. Setter starttid til {selectedTime:HH:mm}.");
                    break;
                }

                if (timeInput.Length == 4 && int.TryParse(timeInput, out int parsedTime))
                {
                    selectedTime = new DateTime(now.Year, now.Month, now.Day, parsedTime / 100, parsedTime % 100, 0);
                    break;
                }

                if (DateTime.TryParseExact(timeInput, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedTime))
                {
                    break;
                }

                Console.WriteLine("❌ Ugyldig tid. Bruk formatet HH:mm eller 4 siffer (f.eks. 1400).");
            }

            // ✅ Get Date Input
            while (true)
            {
                Console.Write(dateMessage);
                dateInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(dateInput) || dateInput.ToLower() == "i dag")
                {
                    selectedTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hour, selectedTime.Minute, 0);
                    break;
                }

                if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    selectedTime = new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, selectedTime.Hour, selectedTime.Minute, 0);
                    break;
                }

                Console.WriteLine("❌ Ugyldig dato. Bruk formatet dd.MM.yyyy eller skriv 'i dag'.");
            }

            return selectedTime;
        }

        public DateTime? GetTimeOrInfinite(string timeMessage, string dateMessage, DateTime startTime)
        {
            DateTime? selectedTime = null;
            string timeInput, dateInput;

            // ✅ Get Time Input (Allow Blank for Infinite)
            while (true)
            {
                Console.Write(timeMessage);
                timeInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(timeInput))
                {
                    Console.WriteLine("⏳ Ingen slutttid oppgitt. Møtet varer uendelig.");
                    return null; // ✅ Infinite Meeting
                }

                if (timeInput.Length == 4 && int.TryParse(timeInput, out int parsedTime))
                {
                    selectedTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, parsedTime / 100, parsedTime % 100, 0);
                    break;
                }

                if (DateTime.TryParseExact(timeInput, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTimeExact))
                {
                    selectedTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, parsedTimeExact.Hour, parsedTimeExact.Minute, 0);
                    break;
                }

                Console.WriteLine("❌ Ugyldig tid. Bruk formatet HH:mm eller 4 siffer (f.eks. 1500).");
            }

            // ✅ Get Date Input (If Needed)
            while (true)
            {
                Console.Write(dateMessage);
                dateInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(dateInput) || dateInput.ToLower() == "i dag")
                {
                    selectedTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, selectedTime.Value.Hour, selectedTime.Value.Minute, 0);
                    break;
                }

                if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    selectedTime = new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, selectedTime.Value.Hour, selectedTime.Value.Minute, 0);
                    break;
                }

                Console.WriteLine("❌ Ugyldig dato. Bruk formatet dd.MM.yyyy eller skriv 'i dag'.");
            }

            return selectedTime;
        }
    }
}
