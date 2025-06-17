using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sprava_domacich_mazlicku
{
    public class Mazlicek
    {

        public string Jmeno { get; set; }
        public string Druh { get; set; }
        private int _vek; // pomocná proměnná pro validaci věku
        public int Vek
        {
            get => _vek;
            set
            {
                // validace nezápornosti věku
                if (value < 0)
                    throw new ArgumentException("Věk mazlíčka nesmí být záporný (nulový věk je povolen).");
                _vek = value;
            }
        }
        public string OblibeneJidlo { get; set; }

        // konstruktor s kontrolou vstupu od uživatele
        public Mazlicek(string vstup)
        {

            // očekáváný formát: ADD;Jáchym;chameleon;9;mouchy
            string[] casti = vstup.Split(';');
            if (casti.Length != 5)
            {
                throw new ArgumentException("Neplatný formát. Zadej příkaz ve tvaru: ADD;[jméno];[druh];[věk];[oblíbené jídlo]");
            }

            Jmeno = casti[1].Trim();
            Druh = casti[2].Trim();

            if (!int.TryParse(casti[3].Trim(), out int vekMazlicka))
            {
                throw new ArgumentException("Věk mazlíčka není ve správném formátu (nezáporné celé číslo).");
            }

            Vek = vekMazlicka; // validace nazápornosti veku proběhne v setteru

            OblibeneJidlo = casti[4].Trim();


        }

        // Metoda pro zobrazení všech informací pod sebou
        public void ZobrazInformace()
        {
            Console.WriteLine($"Jméno: {Jmeno}");
            Console.WriteLine($"Druh: {Druh}");
            Console.WriteLine($"Věk: {Vek} let");
            Console.WriteLine($"Oblíbené jídlo: {OblibeneJidlo}");
        }

        // Info o mazlovi na jednom řádku
        public string VypisInfo()
        {
            return $"{Jmeno} - druh: {Druh}, věk: {Vek} (roky), oblíbené jídlo: {OblibeneJidlo}";
        }

        // Metoda pro zestárnutí mazlíčka
        public void Zestarnout()
        {
            Vek++;
            Console.WriteLine($"\nTak {Jmeno} je zase o rok starší... Už je mu {Vek} let.");
        }
    }
}