using System;
using MeetingPlanner.Services;

class Program
{
    static void Main()
    {
        var meetingService = new MeetingService(); // Use camelCase for variable name

        while (true)
        {
            Console.WriteLine("\n===== 📅 MØTEPLANLEGGER =====");
            Console.WriteLine("1. ➕ Legg til et møte");
            Console.WriteLine("2. 📜 Vis alle møter");
            Console.WriteLine("3. 🔍 Se detaljer for et møte");
            Console.WriteLine("4. ❌ Slett et møte");
            Console.WriteLine("5. ✏️ Rediger et møte");
            Console.WriteLine("6. 🚪 Avslutt");
            Console.Write("👉 Velg et alternativ: ");

            var choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    meetingService.AddMeeting();
                    break;
                case "2":
                    meetingService.DisplayMeetings();
                    break;
                case "3":
                    meetingService.ViewMeetingDetails();
                    break;
                case "4":
                    meetingService.DeleteMeeting();
                    break;
                case "5":
                    meetingService.EditMeeting();
                    break;
                case "6":
                    Console.WriteLine("\n👋 Ha en fin dag!");
                    return;
                default:
                    Console.WriteLine("\n❌ Ugyldig valg! Prøv igjen.");
                    break;
            }
        }
    }
}
