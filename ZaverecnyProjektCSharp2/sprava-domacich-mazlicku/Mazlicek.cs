using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sprava_domacich_mazlicku
{
    public class Mazlicek
    {
        // Vlastnosti mazlíčka
        public string Jmeno { get; set; }
        public string Druh { get; set; }
        public int Vek { get; set; }
        public string OblibeneJidlo { get; set; }

        // Konstruktor
        public Mazlicek(string jmeno, string druh, int vek, string oblibeneJidlo)
        {
            Jmeno = jmeno;
            Druh = druh;
            Vek = vek;
            OblibeneJidlo = oblibeneJidlo;
        }

        // Metoda na zobrazení informací
        public void ZobrazInformace()
        {
            Console.WriteLine($"Jméno: {Jmeno}");
            Console.WriteLine($"Druh: {Druh}");
            Console.WriteLine($"Věk: {Vek} let");
            Console.WriteLine($"Oblíbené jídlo: {OblibeneJidlo}");
        }

        // Metoda pro zestárnutí mazlíčka
        public void Zestarnout()
        {
            Vek++;
            Console.WriteLine($"\nTak {Jmeno} je zase o rok starší... Už je mu {Vek} let.");
        }
    }
}