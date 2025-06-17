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
            // vytvoreni prvniho mazlicka
            Mazlicek pesBertik = new Mazlicek("Bertík", "pes", 3, "masíčko");

            // Zobrazení informací
            pesBertik.ZobrazInformace();

            pesBertik.Zestarnout();

            // Znovu zobrazíme informace
            pesBertik.ZobrazInformace();
        }
    }
}

