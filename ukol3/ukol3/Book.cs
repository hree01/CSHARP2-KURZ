using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ukol3
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        // pomocna privatni promenna:
        private int _pages;
        public int Pages
        {
            get => _pages;
            set
            {
                // validace
                if (value <= 0)
                    throw new ArgumentException("Počet stran musí být větší než 0.");
                _pages = value;
            }
        }

        public Book(string vstup)
        {
            // očekáváný formát: ADD;1984;George Orwell;1949-06-08;328
            string[] casti = vstup.Split(';');
            if (casti.Length != 5)
            {
                throw new ArgumentException("Neplatný formát. Zadej příkaz ve tvaru: ADD;název knihy;autor;datum vydání ve formátu YYYY-MM-DD;počet stran");
            }

            Title = casti[1].Trim();
            Author = casti[2].Trim();

            if (!DateTime.TryParse(casti[3].Trim(), out DateTime datumVydani))
            {
                throw new ArgumentException("Datum není ve správném formátu YYYY-MM-DD.");
            }

            PublishedDate = datumVydani;

            if (!int.TryParse(casti[4].Trim(), out int pocetStran))
            {
                throw new ArgumentException("Počet stran musí být celé číslo.");
            }

            Pages = pocetStran; // validace nazápornosti počtu stran proběhne v setteru
        }

        public string PopisKnihy()
        {
            // Kniha: 1984, autor: George Orwell, vydáno 8.6.1949, stran: 328
            return $"Kniha: {Title}, autor: {Author}, vydáno {PublishedDate.ToString("dd.MM.yyyy")}, stran: {Pages}";
        }

    }
}