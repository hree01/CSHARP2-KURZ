using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sprava_domacich_mazlicku
{
    class Program
    {
        static void Main()
        {
            // Vytvoření prvního mazlíčka
            Mazlicek pesBertik = new Mazlicek("ADD;Bertík;pes;3;masíčko");

            // Zobrazení informací
            pesBertik.ZobrazInformace();

            // Zestárnutí        
            pesBertik.Zestarnout();

            // Kontrola informací
            pesBertik.ZobrazInformace();

            List<Mazlicek> mazlicci = new List<Mazlicek>();
            string vstup;

            Console.WriteLine("- Přidej mazlíčka do databáze: ADD;[jméno];[druh];[věk];[oblíbené jídlo]\n"
                        + "- Vypiš všechny mazlíčky: LIST\n"
                        + "- Vypiš statistiky: STATS\n"
                        + "- Hledej podle klíčového slova (jméno nebo druh mazla): FIND;[klíčové slovo]\n"
                        + "- Ukonči program: END\n");

            while (true)
            {
                vstup = Console.ReadLine().Trim();

                // při omylem zadaném prázdném vstupu se nic nestane, program pokračuje dál
                if (string.IsNullOrWhiteSpace(vstup))
                    continue;

                if (vstup.ToUpper() == "END")
                {
                    Console.WriteLine("Program ukončen.");
                    break;
                }
                else if (vstup.ToUpper() == "LIST")
                {
                    if (mazlicci.Count == 0)
                    {
                        Console.WriteLine("Zatím žádní mazlíčci. Přidej nějakého.");
                        continue;
                    }
                    // výpis mazlíčků podle jmen
                    foreach (var mazel in mazlicci.OrderBy(m => m.Jmeno))
                    {
                        Console.WriteLine(mazel.VypisInfo());
                    }
                }
                else if (vstup.ToUpper() == "STATS")
                {
                    if (mazlicci.Count == 0)
                    {
                        Console.WriteLine("Žádné knihy nejsou uloženy - nejsou ani statistiky.");
                        continue;
                    }

                    // počet mazlů
                    int pocetMazliku = mazlicci.Count;
                    Console.WriteLine($"\nStatistiky:\nPočet zapsaných mazlíčků: {pocetMazliku}");

                    // průměrný věk (pro přesnost v double)
                    double prumernyVek = mazlicci.Select(m => m.Vek).Average();
                    // vysledek zaokrouhlim na cele cislo
                    Console.WriteLine($"Průměrný věk zapsaných mazlíčků: {Math.Round(prumernyVek)}");



                    Console.WriteLine("Počet mazlíčků podle druhu:");
                    // seskupím všechny mazlíčky podle druhu -> vrati se neco jako dictionary
                    // akorat s jednim klicem pro vice hodnot
                    // druh = klic, jmena mazlíčků = hodnoty (seznam hodnot)
                    var podleDruhu = mazlicci.GroupBy(m => m.Druh);

                    foreach (var skupina in podleDruhu)
                    {
                        Console.WriteLine($" - {skupina.Key}: {skupina.Count()}");
                    }

                }
                else if (vstup.StartsWith("FIND", StringComparison.OrdinalIgnoreCase))
                {
                    string[] casti = vstup.Split(';');
                    if (casti.Length != 2 || string.IsNullOrWhiteSpace(casti[1]))
                    {
                        Console.WriteLine("Neplatný formát příkazu FIND. Zadej příkaz ve tvaru FIND;slovo");
                        continue;
                    }

                    string hledane = casti[1].Trim().ToLower();

                    var nalezeniMazlicci = mazlicci
                        // kontrola všech jmen a druhů mazlíčků, jesli neobsahují hledaný řetězec:
                        .Where(m => m.Jmeno.ToLower().Contains(hledane) || m.Druh.ToLower().Contains(hledane))
                        // zařazení do seznamu, který bude v dalším kroku vypsán
                        .ToList();

                    Console.WriteLine($"\nVýsledky hledání pro \"{hledane}\":");
                    foreach (Mazlicek mazlicek in nalezeniMazlicci)
                    {
                        Console.WriteLine($" - {mazlicek.VypisInfo()}");
                    }

                    if (!nalezeniMazlicci.Any())
                        Console.WriteLine(" - Nenalezeno.");
                }
                else if (vstup.StartsWith("ADD", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Mazlicek novyMazel = new Mazlicek(vstup);
                        mazlicci.Add(novyMazel);
                        Console.WriteLine("Mazlíček byl úspěšně přidán do seznamu.");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Chyba: " + exception.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Neznámý příkaz. Seznam povolených příkazů:\n"
                        + "- Přidej mazlíčka: ADD;[jméno];[druh];[věk];[oblíbené jídlo]\n"
                        + "- Vypiš všechny mazlíčky: LIST\n"
                        + "- Vypiš statistiky: STATS\n"
                        + "- Hledej podle klíčového slova (jméno nebo druh mazla): FIND;[klíčové slovo]\n"
                        + "- Ukonči program: END\n");
                }
            }

        }
    }
}

