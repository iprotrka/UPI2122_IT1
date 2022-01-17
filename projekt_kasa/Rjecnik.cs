using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_kasa
{
    class Rjecnik : Dictionary<int, Pice>
    {
        public string TraziPoSlovu(char slovo)
        {
            string ispis = "";
            Dictionary<int, Pice> ovaj = this;

            foreach (int k in ovaj.Keys)
            {
                Pice neki = (Pice)ovaj[k];
                if (slovo == (char)ovaj[k].Naziv.ToUpper()[0])
                    ispis += ovaj[k].Naziv + "\n";
            }

            if (ispis != "")
                return ispis;
            else
                return "nema takvih naziva";
        }
    }
}
