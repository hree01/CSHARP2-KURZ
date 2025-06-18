using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace sprava_domacich_mazlicku
{
    class Program
    {
        static void Main()
        {

            List<Mazlicek> mazlicci = new List<Mazlicek>();
            string vstup;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Vítej ve správci domácích mazlíčků!");
            Console.ResetColor();

            Console.WriteLine("- Přidej mazlíčka do databáze: ADD;[jméno];[druh];[věk];[oblíbené jídlo]\n"
                        + "- Změň jméno mazlíčka: CHNAME;[původní jméno];[druh];[nové jméno]\n"
                        + "- Změň druh mazlíčka: CHTYPE;[jméno];[původní druh];[nový druh]\n"
                        + "- Přidej mazlíčkovi rok k věku: TOAGE;[jméno];[druh]\n"
                        + "- Prohlédni si info o mazlíčkovi: INFO;[jméno]\n"
                        + "- Vymaž mazlíčka ze seznamu: DELETE;[jméno];[druh]\n"
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
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Program ukončen.");
                    Console.ResetColor();
                    break;
                }

                else if (vstup.StartsWith("CHNAME", StringComparison.OrdinalIgnoreCase))
                {
                    // očekáváný formát: CHNAME;[původní jméno];[druh];[nové jméno]
                    string[] casti = vstup.Split(';');

                    if (casti.Length != 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát. Zadej příkaz ve tvaru: CHNAME;[původní jméno];[druh];[nové jméno]");
                        Console.ResetColor();
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(casti[1]) || string.IsNullOrWhiteSpace(casti[2]) || string.IsNullOrWhiteSpace(casti[3]))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Všechna pole musí být vyplněna. Zadej příkaz ve tvaru: CHNAME;[původní jméno];[druh];[nové jméno]");
                        Console.ResetColor();
                        continue;
                    }
                    string stareJmeno = casti[1].Trim();
                    string druh = casti[2].Trim();
                    string noveJmeno = casti[3].Trim();

                    // hledám mazla v seznamu
                    var mazlicek = Mazlicek.Najdi(mazlicci, stareJmeno, druh);
                    // když tam není, upozorním uživatele
                    if (mazlicek == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Mazlíček {stareJmeno} ({druh}) nebyl nalezen.");
                        Console.ResetColor();
                        continue;
                    }

                    mazlicek.ZmenJmeno(noveJmeno);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Jméno mazlíčka {stareJmeno} ({druh}) bylo změněno na {noveJmeno}.");
                    Console.ResetColor();
                }
                else if (vstup.StartsWith("CHTYPE", StringComparison.OrdinalIgnoreCase))
                {
                    // očekáváný formát: CHTYPE;[jméno];[původní druh];[nový druh]
                    string[] casti = vstup.Split(';');
                    if (casti.Length != 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát. Zadej příkaz ve tvaru: CHTYPE;[jméno];[původní druh];[nový druh]");
                        Console.ResetColor();
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(casti[1]) || string.IsNullOrWhiteSpace(casti[2]) || string.IsNullOrWhiteSpace(casti[3]))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Všechna pole musí být vyplněna. Zadej příkaz ve tvaru: CHTYPE;[jméno];[původní druh];[nový druh]");
                        Console.ResetColor();
                        continue;
                    }
                    string jmeno = casti[1].Trim();
                    string staryDruh = casti[2].Trim();
                    string novyDruh = casti[3].Trim();

                    var mazlicek = Mazlicek.Najdi(mazlicci, jmeno, staryDruh);
                    if (mazlicek == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Mazlíček {jmeno} ({staryDruh}) nebyl nalezen.");
                        Console.ResetColor();
                        continue;
                    }

                    mazlicek.ZmenDruh(novyDruh);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Druh mazlíčka {jmeno} ({staryDruh}) byl změněn na {novyDruh}.");
                    Console.ResetColor();
                }
                else if (vstup.StartsWith("TOAGE", StringComparison.OrdinalIgnoreCase))
                {
                    // očekáváný formát: TOAGE;[jméno];[druh]
                    string[] casti = vstup.Split(';');
                    if (casti.Length != 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát. Zadej příkaz ve tvaru: TOAGE;[jméno];[druh]");
                        Console.ResetColor();
                        continue;
                    }
                    string jmeno = casti[1].Trim();
                    string druh = casti[2].Trim();

                    var mazlicek = Mazlicek.Najdi(mazlicci, jmeno, druh);
                    if (mazlicek == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Mazlíček {jmeno} ({druh}) nebyl nalezen.");
                        Console.ResetColor();
                        continue;
                    }

                    mazlicek.Zestarnout();

                }
                else if (vstup.StartsWith("INFO", StringComparison.OrdinalIgnoreCase))
                {
                    // očekáváný formát: INFO;[jméno]
                    string[] casti = vstup.Split(';');
                    if (casti.Length != 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát. Zadej příkaz ve tvaru: INFO;[jméno]");
                        Console.ResetColor();
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(casti[1]))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Nezadal jsi jméno mazlíčka, o kterém chceš zobrazit info.");
                        Console.ResetColor();
                        continue;
                    }

                    string hledaneJmeno = casti[1].Trim().ToLower();

                    var nalezeniMazlicci = mazlicci
                        // kontrola všech jmen mazlíčků, jesli se rovnají hledanému:
                        .Where(m => m.Jmeno.ToLower().Equals(hledaneJmeno))
                        // zařazení do seznamu, který bude v dalším kroku vypsán
                        .ToList();

                    Console.WriteLine($"\nMazlíček/mazlíčci se jménem \"{casti[1]}\":");
                    foreach (Mazlicek mazlicek in nalezeniMazlicci)
                    {
                        mazlicek.ZobrazInformace();
                    }

                    if (!nalezeniMazlicci.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"...\nMazlíček se jménem {casti[1]} nenalezen.");
                        Console.ResetColor();
                    }

                }
                else if (vstup.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                {
                    // očekáváný formát: DELETE;[jméno];[druh]
                    string[] casti = vstup.Split(';');

                    if (casti.Length != 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát. Zadej příkaz ve tvaru: DELETE;[jméno];[druh]");
                        Console.ResetColor();
                        continue;
                    }

                    string jmeno = casti[1].Trim();
                    string druh = casti[2].Trim();

                    if (string.IsNullOrWhiteSpace(jmeno) || string.IsNullOrWhiteSpace(druh))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Musíš zadat jak jméno, tak druh mazlíčka.");
                        Console.ResetColor();
                        continue;
                    }

                    var mazlicek = Mazlicek.Najdi(mazlicci, jmeno, druh);

                    if (mazlicek == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Mazlíček {jmeno} ({druh}) nebyl nalezen, takže nemohl být smazán.");
                        Console.ResetColor();
                        continue;
                    }

                    Console.WriteLine($"Skutečně odstranit mazlíčka {jmeno} ({druh}) ze seznamu? Pro potvrzení zadej \"y\"");
                    var potvrzeni = Console.ReadLine();
                    if (potvrzeni != null && potvrzeni.ToLower().Equals("y"))
                    {
                        mazlicci.Remove(mazlicek);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Mazlíček {jmeno} ({druh}) byl úspěšně odebrán ze seznamu.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Nepotvrdil jsi operaci. Mazlíček {jmeno} ({druh}) NEBYL odebrán ze seznamu.");
                        Console.ResetColor();
                    }

                }
                else if (vstup.ToUpper() == "LIST")
                {
                    if (mazlicci.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Zatím žádní mazlíčci. Přidej nějakého.");
                        Console.ResetColor();
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
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"\nStatistiky:");
                    Console.ResetColor();
                    Console.WriteLine($"\nPočet zapsaných mazlíčků: {pocetMazliku}");

                    // průměrný věk (pro přesnost v double)
                    double prumernyVek = mazlicci.Select(m => m.Vek).Average();
                    // vysledek zaokrouhlim na cele cislo
                    Console.WriteLine($"Průměrný věk zapsaných mazlíčků: {prumernyVek}");



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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Neplatný formát příkazu FIND. Zadej příkaz ve tvaru FIND;slovo");
                        Console.ResetColor();
                        continue;
                    }

                    string hledane = casti[1].Trim().ToLower();

                    var nalezeniMazlicci = mazlicci
                        // kontrola všech jmen a druhů mazlíčků, jesli neobsahují hledaný řetězec:
                        .Where(m => m.Jmeno.ToLower().Contains(hledane) || m.Druh.ToLower().Contains(hledane))
                        // zařazení do seznamu, který bude v dalším kroku vypsán
                        .ToList();

                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"\nVýsledky hledání pro \"{hledane}\":");
                    Console.ResetColor();
                    foreach (Mazlicek mazlicek in nalezeniMazlicci)
                    {
                        Console.WriteLine($" - {mazlicek.VypisInfo()}");
                    }

                    if (!nalezeniMazlicci.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" - Nenalezeno.");
                        Console.ResetColor();
                    }
                }
                else if (vstup.StartsWith("ADD", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Mazlicek novyMazel = new Mazlicek(vstup);

                        // Kontrola - nový mazel nesmí mít stejné jméno a zároveň druh s již existujícími mazlíčky
                        bool existujeTakovyMazlicek = mazlicci.Any(m =>
                                        m.Jmeno.Equals(novyMazel.Jmeno, StringComparison.OrdinalIgnoreCase) &&
                                        m.Druh.Equals(novyMazel.Druh, StringComparison.OrdinalIgnoreCase));

                        if (existujeTakovyMazlicek)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Mazlíček se jménem {novyMazel.Jmeno} a druhem {novyMazel.Druh} už existuje. Vždyť by byli všichni zmatení. Zkus to jinak.");
                            Console.ResetColor();
                            continue;
                            // TODO: Momentálně mazlíček bude pořád v paměti programu, i když není v seznamu - musím dořešit později. Ale měl by se o to postarat .NET garbage collector
                        }

                        // Když je všechno v pořádku, přidám mazlíka do seznamu
                        mazlicci.Add(novyMazel);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Mazlíček byl úspěšně přidán do seznamu.");
                        Console.ResetColor();
                    }
                    catch (Exception exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Chyba: " + exception.Message);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("Neznámý příkaz. Seznam povolených příkazů:\n"
                        + "- Přidej mazlíčka: ADD;[jméno];[druh];[věk];[oblíbené jídlo]\n"
                        + "- Změň jméno mazlíčka: CHNAME;[původní jméno];[druh];[nové jméno]\n"
                        + "- Změň druh mazlíčka: CHTYPE;[jméno];[původní druh];[nový druh]\n"
                        + "- Přidej mazlíčkovi rok k věku: TOAGE;[jméno];[druh]\n"
                        + "- Prohlédni si info o mazlíčkovi: INFO;[jméno]\n"
                        + "- Vymaž mazlíčka ze seznamu: DELETE;[jméno];[druh]\n"
                        + "- Vypiš všechny mazlíčky: LIST\n"
                        + "- Vypiš statistiky: STATS\n"
                        + "- Hledej podle klíčového slova (jméno nebo druh mazla): FIND;[klíčové slovo]\n"
                        + "- Ukonči program: END\n");
                }
            }

        }
    }
}

