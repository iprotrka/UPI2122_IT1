using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt_kasa
{
    class Pice
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private string naziv;
        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }
        private double cijena;
        public double Cijena
        {
            get { return cijena; }
            set { cijena = value; }
        }
        public Pice(string n, double c,int i)
        {
            
            this.Naziv = n;
            this.Cijena = c;
            this.ID = i;
        }
    }
}
