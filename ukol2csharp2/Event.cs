using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ukol2csharp2
{
    public class Event
    {
        // Vytvořte třídu Event, která bude uchovávat informace o události (jméno a datum). Použijte funkci string.Split pro získání jména akce a jednotlivých částí data: rok, měsíc a den (funkci string.Split můžete použít i vícekrát – jednou pro získání jména a celého data a znovu pro parsování data). Části data použijte k vytvoření objektu DateTime. Pomocí získaných údajů vytvořte objekt typu Event a uložte ho do připraveného seznamu (List).
        public string Jmeno { get; set; }
        public DateTime Datum { get; set; }

        public Event(string vstup)
        {
            // očekáváný formát: EVENT;Jmeno;Datum
            string[] casti = vstup.Split(';');
            if (casti.Length != 3)
            {
                throw new ArgumentException("Neplatný formát. Zadej příkaz ve tvaru: EVENT;jmeno udalosti;yyyy-mm-dd");
            }

            Jmeno = casti[1].Trim();

            if (!DateTime.TryParse(casti[2].Trim(), out DateTime datum))
            {
                throw new ArgumentException("Datum není ve správném formátu yyyy-mm-dd.");
            }

            Datum = datum;
        }

        public string PopisUdalosti()
        {
            int zaKolikDni = (Datum - DateTime.Today).Days;

            if (zaKolikDni > 0)
                return $"Event {Jmeno} with date {Datum.ToString("yyyy-MM-dd")} will happen in {zaKolikDni} days";
            else if (zaKolikDni < 0)
                return $"Event {Jmeno} with date {Datum.ToString("yyyy-MM-dd")} happened {-zaKolikDni} days ago";
            else
                return $"Event {Jmeno} is happening today ({Datum.ToString("yyyy-MM-dd")})";
        }

    }
}