using System;
using MeetingPlanner.Services;

class Program
{
    static void Main()
    {
        MeetingService MeetingService = new MeetingService();

        while (true)
        {
            Console.WriteLine("\n===== MØTEPLANLEGGER =====");
            Console.WriteLine("1. Legg til et møte");
            Console.WriteLine("2. Vis alle møter");
            Console.WriteLine("3. Se detaljer for et møte");
            Console.WriteLine("4. Slett et møte");
            Console.WriteLine("5. Rediger et møte");
            Console.WriteLine("6. Avslutt");
            Console.Write("Velg et alternativ: ");

            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    MeetingService.AddMeeting();
                    break;
                case "2":
                    MeetingService.DisplayMeetings();
                    break;
                case "3":
                    MeetingService.ViewMeetingDetails();
                    break;
                case "4":
                    MeetingService.DeleteMeeting();
                    break;
                case "5":
                    MeetingService.EditMeeting();  // ✅ Ensuring EditMeeting is referenced correctly
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