
// Program bude opakovaně číst vstup z konzole. Vstup může být jeden z následujících:

// ADD;[název];[autor];[datum vydání ve formátu YYYY-MM-DD];[počet stran]
// Např.: ADD;1984;George Orwell;1949-06-08;328

// LIST
// Vypíše všechny knihy, seřazené podle data vydání. Použijte OrderBy

// STATS
// Vypíše:
// Průměrný počet stran (použijte Select a Average)
// Počet knih od každého autora (použijte GroupBy)
// Počet unikatních slov v názvech knih. Použijte SelectMany a rozdělení názvů podle mezer (interpunkci vynechte) pro vytvoření jednoho seznamu všech slov, pak použijte Distinct.

// FIND;[klíčové slovo]
// Vyhledá knihy, jejichž název obsahuje dané slovo, bez ohledu na velikost písmen (použijte Where).

// END
// Ukončí program.

namespace ukol3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Book> knihy = new List<Book>();
            string vstup;

            Console.WriteLine("- Přidej knihu: ADD;[název];[autor];[datum vydání ve formátu YYYY-MM-DD];[počet stran]\n"
                        + "- Vypiš všechny knihy: LIST\n"
                        + "- Vypiš statistiky: STATS\n"
                        + "- Hledej podle klíčového slova v názvu knihy: FIND;[klíčové slovo]\n"
                        + "- Ukonči program: END\n");

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
                    if (knihy.Count == 0)
                    {
                        Console.WriteLine("Žádné knihy nejsou uloženy.");
                        continue;
                    }
                    // vypisu knihy vzestupne podle data
                    foreach (var kniha in knihy.OrderBy(k => k.PublishedDate))
                    {
                        Console.WriteLine(kniha.PopisKnihy());
                    }
                }
                else if (vstup.ToUpper() == "STATS")
                {
                    if (knihy.Count == 0)
                    {
                        Console.WriteLine("Žádné knihy nejsou uloženy - nejsou ani statistiky.");
                        continue;
                    }

                    // pro lepsi presnost nejprve pouziju typ double
                    double prumerStran = knihy.Select(k => k.Pages).Average();
                    // vysledek zaokrouhlim na cele cislo
                    Console.WriteLine($"\nStatistiky:\nPrůměrný počet stran: {Math.Round(prumerStran)}");

                    Console.WriteLine("Počet knih podle autora:");
                    // seskupim vsechny knihy podle jmena autora -> vrati se neco jako dictionary
                    // akorat s jednim klicem pro vice hodnot
                    // autor = klic, knihy = hodnoty (seznam hodnot)
                    var podleAutoru = knihy.GroupBy(k => k.Author);

                    foreach (var skupina in podleAutoru)
                    {
                        Console.WriteLine($" - {skupina.Key}: {skupina.Count()}");
                    }

                    var unikatniSlova = knihy
                        // vsechny nazvy knih rozdelim do jednotlivych slov (podle mezery)
                        // pak z nich vznikne seznam:
                        .SelectMany(k => k.Title.Split([' '], StringSplitOptions.RemoveEmptyEntries))
                        // odstranim interpunkci prilepenou na koncich nebo zacatcich slov:
                        .Select(slovo => slovo.Trim(['.', ',', ';', '!', '?', ':', '-']))
                        // kdyby po odstraneni interpunkce vznikl prazdny retezec, vyradi se:
                        .Where(slovo => !string.IsNullOrWhiteSpace(slovo))
                        // prevedu vsechna slova na mala pismena kvůli porovnavani:
                        .Select(slovo => slovo.ToLower())
                        // odstranim slova, ktera jsou tam vickrat:
                        .Distinct();
                    // na zaver slova spocitam:
                    Console.WriteLine($"\nPočet unikátních slov v názvech knih: {unikatniSlova.Count()}\n");
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

                    var nalezene = knihy
                        // u vseh knih se podivam, jestli jejich nazev neobsahuje hledany retezec:
                        .Where(k => k.Title.ToLower().Contains(hledane))
                        // a kdyz jo, tak si vyberu cely nazev te knihy a ulozim ho do seznamu "nalezene":
                        .Select(k => k.Title);

                    Console.WriteLine($"\nVýsledky hledání pro \"{hledane}\":");
                    foreach (var nazev in nalezene)
                    {
                        Console.WriteLine($" - {nazev}");
                    }

                    if (!nalezene.Any())
                        Console.WriteLine(" - Nenalezeno.");
                }
                else if (vstup.StartsWith("ADD", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Book novaKniha = new Book(vstup);
                        knihy.Add(novaKniha);
                        Console.WriteLine("Kniha byla úspěšně přidána.");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Chyba: " + exception.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Neznámý příkaz. Seznam povolených příkazů:\n"
                        + "- Přidej knihu: ADD;[název];[autor];[datum vydání ve formátu YYYY-MM-DD];[počet stran]\n"
                        + "- Vypiš všechny knihy: LIST\n"
                        + "- Vypiš statistiky: STATS\n"
                        + "- Hledej podle klíčového slova v názvu knihy: FIND;[klíčové slovo]\n"
                        + "- Ukonči program: END\n");
                }
            }
        }
    }
}
