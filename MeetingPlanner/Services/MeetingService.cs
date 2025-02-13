using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using MeetingPlanner.Models;

namespace MeetingPlanner.Services
{
    public class MeetingService
    {
        private readonly List<Meeting> møter = new();
        private readonly string loggFil = "møter.json"; // JSON-fil for lagring

        public void LeggTilMøte()
        {
            Console.WriteLine("\n📅 Nytt møte");

            Console.Write("Tittel: ");
            string tittel = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Sted: ");
            string sted = Console.ReadLine()?.Trim() ?? "";

            DateTime startTid = HentTidOgDato("Starttid (HH:mm eller 4 siffer, f.eks. 1400): ", "Startdato (dd.MM.yyyy eller 'i dag'): ");

            // Slutttid kan være tom, da blir møtet "uendelig"
            DateTime? sluttTid = HentTidEllerUendelig("Slutttid (HH:mm eller 4 siffer, f.eks. 1500, eller blank for uendelig): ", "Sluttdato (dd.MM.yyyy eller 'i dag'): ", startTid);

            Console.Write("Hvem oppretter møtet? ");
            string opprettetAv = Console.ReadLine()?.Trim() ?? "Ukjent";

            Console.Write("Kort beskrivelse: ");
            string beskrivelse = Console.ReadLine()?.Trim() ?? "";

            // Legg til deltakere
            List<string> deltakere = new();
            Console.WriteLine("Skriv inn deltakere (trykk Enter for å stoppe): ");
            while (true)
            {
                Console.Write("Deltaker: ");
                string? deltaker = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(deltaker)) break;
                deltakere.Add(deltaker);
            }

            var møte = new Meeting
            {
                Id = møter.Count + 1,
                Tittel = tittel,
                StartTid = startTid,
                SluttTid = sluttTid, // Kan være null hvis det ikke er satt en slutt
                Sted = sted,
                Beskrivelse = beskrivelse,
                OpprettetAv = opprettetAv,
                Deltakere = deltakere
            };

            møter.Add(møte);
            LoggTilFil();
            Console.WriteLine($"\n✅ Møte '{tittel}' lagt til!\n");
        }

        private DateTime HentTidOgDato(string tidMelding, string datoMelding, DateTime? etterDato = null)
        {
            while (true)
            {
                Console.Write(tidMelding);
                string? tidInput = Console.ReadLine()?.Trim();

                if (tidInput?.Length == 4 && int.TryParse(tidInput, out _))
                {
                    tidInput = tidInput.Insert(2, ":");
                }

                if (TimeSpan.TryParseExact(tidInput, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan klokkeslett))
                {
                    Console.Write(datoMelding);
                    string? datoInput = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(datoInput) || datoInput.ToLower() == "i dag" || datoInput.ToLower() == "idag")
                    {
                        return DateTime.Today.Add(klokkeslett);
                    }

                    if (DateTime.TryParseExact(datoInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dato))
                    {
                        if (etterDato == null || dato.Add(klokkeslett) > etterDato)
                        {
                            return dato.Add(klokkeslett);
                        }
                    }
                }
            }
        }

        private DateTime? HentTidEllerUendelig(string tidMelding, string datoMelding, DateTime etterDato)
        {
            while (true)
            {
                Console.Write(tidMelding);
                string? tidInput = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(tidInput)) return null; // Uendelig møte

                if (tidInput.Length == 4 && int.TryParse(tidInput, out _))
                {
                    tidInput = tidInput.Insert(2, ":");
                }

                if (TimeSpan.TryParseExact(tidInput, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan klokkeslett))
                {
                    Console.Write(datoMelding);
                    string? datoInput = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(datoInput) || datoInput.ToLower() == "i dag" || datoInput.ToLower() == "idag")
                    {
                        return DateTime.Today.Add(klokkeslett);
                    }

                    if (DateTime.TryParseExact(datoInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dato))
                    {
                        if (dato.Add(klokkeslett) > etterDato)
                        {
                            return dato.Add(klokkeslett);
                        }
                    }
                }
            }
        }

        public void SlettMøte()
        {
            LastInnFraFil();

            if (møter.Count == 0)
            {
                Console.WriteLine("\n❌ Ingen møter å slette.");
                return;
            }

            Console.WriteLine("\n📅 Velg et møte å slette:");
            for (int i = 0; i < møter.Count; i++)
            {
                Console.WriteLine($"[{møter[i].Id}] {møter[i].Tittel} - {møter[i].StartTid:dd.MM.yyyy HH:mm}");
            }

            Console.Write("\nSkriv ID på møtet du vil slette: ");
            if (int.TryParse(Console.ReadLine(), out int møteId))
            {
                var møte = møter.Find(m => m.Id == møteId);
                if (møte != null)
                {
                    møter.Remove(møte);
                    LoggTilFil();
                    Console.WriteLine($"✅ Møte '{møte.Tittel}' er slettet.");
                }
                else
                {
                    Console.WriteLine("❌ Fant ikke møtet med den ID-en.");
                }
            }
            else
            {
                Console.WriteLine("❌ Ugyldig ID.");
            }
        }

        private void LoggTilFil()
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(møter, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(loggFil, jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Feil ved skriving til fil: {e.Message}");
            }
        }

        public void VisMøter()
        {
            LastInnFraFil();

            if (møter.Count == 0)
            {
                Console.WriteLine("\n❌ Ingen møter planlagt.\n");
                return;
            }

            Console.WriteLine("\n📅 Planlagte møter:");
            foreach (var møte in møter)
            {
                string sluttTid = møte.SluttTid.HasValue ? møte.SluttTid.Value.ToString("dd.MM.yyyy HH:mm") : "Uendelig";
                Console.WriteLine($"- [{møte.Id}] {møte.Tittel} på {møte.Sted} ({møte.StartTid:dd.MM.yyyy HH:mm} - {sluttTid})");
            }
        }

        private void LastInnFraFil()
        {
            try
            {
                if (File.Exists(loggFil))
                {
                    string jsonData = File.ReadAllText(loggFil);
                    var lagredeMøter = JsonSerializer.Deserialize<List<Meeting>>(jsonData);
                    if (lagredeMøter != null)
                    {
                        møter.Clear();
                        møter.AddRange(lagredeMøter);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Feil ved lesing av fil: {e.Message}");
            }
        }
    }
}
