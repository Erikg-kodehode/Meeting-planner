using System;
using MeetingPlanner.Services;

class Program
{
    static void Main()
    {
        MeetingService møteTjeneste = new MeetingService();

        while (true)
        {
            Console.WriteLine("\n===== MØTEPLANLEGGER =====");
            Console.WriteLine("1. Legg til et møte");
            Console.WriteLine("2. Vis møter");
            Console.WriteLine("3. Avslutt");
            Console.Write("Velg et alternativ: ");

            string valg = Console.ReadLine() ?? "";
            switch (valg)
            {
                case "1":
                    møteTjeneste.LeggTilMøte();
                    break;
                case "2":
                    møteTjeneste.VisMøter();
                    break;
                case "3":
                    Console.WriteLine("\nHa en fin dag! 👋");
                    return;
                default:
                    Console.WriteLine("\n❌ Ugyldig valg! Prøv igjen.");
                    break;
            }
        }
    }
}
