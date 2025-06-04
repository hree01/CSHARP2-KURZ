using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Text ve formátu EVENT;[event name];[event date], kde [event name] je jméno události a [event date] je datum ve formátu YYYY-MM-DD, např. EVENT;Lekce Czechitas;2025-05-15.
// Text LIST, který bude znamenat výpis událostí.
// Text STATS, který bude znamenat výpis statistik.
// Text END, který ukončí program.


// EVENT;Lekce Czechitas;2025-05-15
// nejdriv zjistim, co obsahuje vstupni string (pouziju trim na odstraneni mezer), hledam event; (startswith)

using ukol2csharp2;

class Program
{
    static void Main(string[] args)
    {
        List<Event> udalosti = new List<Event>();
        string vstup;

        Console.WriteLine("Zadej událost (EVENT;jméno;datum), LIST, STATS nebo END:");

        while (true)
        {
            vstup = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(vstup))
                continue;

            if (vstup.ToUpper() == "END")
            {
                Console.WriteLine("Program ukončen.");
                break;
            }
            else if (vstup.ToUpper() == "LIST")
            {
                if (udalosti.Count == 0)
                {
                    Console.WriteLine("Žádné události nejsou uloženy.");
                    continue;
                }

                foreach (var udalost in udalosti)
                {
                    Console.WriteLine(udalost.PopisUdalosti());
                }
            }
            else if (vstup.ToUpper() == "STATS")
            {
                if (udalosti.Count == 0)
                {
                    Console.WriteLine("Žádné události nejsou uloženy - nejsou ani statistiky.");
                    continue;
                }
                Dictionary<DateTime, int> statistiky = new Dictionary<DateTime, int>();

                foreach (var udalost in udalosti)
                {
                    if (statistiky.ContainsKey(udalost.Datum))
                        statistiky[udalost.Datum]++;
                    else
                        statistiky[udalost.Datum] = 1;
                }

                foreach (var pair in statistiky)
                {
                    Console.WriteLine($"Date: {pair.Key.ToString("yyyy-MM-dd")}: events: {pair.Value}");
                }
            }
            else if (vstup.StartsWith("EVENT;", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    Event novaUdalost = new Event(vstup);
                    udalosti.Add(novaUdalost);
                    Console.WriteLine("Událost byla úspěšně přidána.");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Chyba: " + exception.Message);
                }
            }
            else
            {
                Console.WriteLine("Neznámý příkaz. Zadej \"EVENT;jmeno udalosti;yyyy-mm-dd\", \"LIST\", \"STATS\" nebo \"END\".");
            }
        }
    }
}
