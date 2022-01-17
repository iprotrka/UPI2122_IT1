using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace projekt_kasa
{
    public partial class Form2 : Form
    {
        Form opener;
        public Form2(Form parentForm)
        {
            InitializeComponent();
            opener = ParentForm;
            //uređivanje zaglavlja richtextboxa( račun )
            rtbRacun.Text = "";
            cmbPice.Items.Clear();
            cmbPice.Text = "";
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("                    CAFFE BAR ENTER                                ");
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("NAZIV" + "/" + "KOLIČINA" + "/" + "CIJENA" + "\n");
        }
        //stvaranje dictionary-a i lista za zbrajanje računa
        Rjecnik dict = new Rjecnik();
        List<double> dnevni_promet = new List<double>();
        List<double> iznos_racuna = new List<double>();
        private void Ispis()//ispis id, naziv, cijena u rtb za brisanje pića
        {
            rtbPopis.Text = "";
            cmbBrojevi.Items.Clear();
            cmbBrojevi.Text = "";
            rtbPopis.AppendText("ID" + "\t" + "NAZIV" + "\t" + "CIJENA" + "\n");
            foreach (int k in dict.Keys)
            {
                Pice neki = (Pice)dict[k];
                rtbPopis.AppendText(neki.ID.ToString()+"\t"+neki.Naziv+"\t"+neki.Cijena.ToString()+"\n");
                cmbBrojevi.Items.Add(k.ToString());
            }
        }
        
        private void Ispis2()//ispis stvaki računa
        {
            rtbRacun.Text = "";
            cmbPice.Items.Clear();
            cmbPice.Text = "";
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("                    CAFFE BAR ENTER                                ");
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("NAZIV" + "/" + "KOLIČINA" + "/" + "CIJENA" + "\n");

            foreach (int k in dict.Keys)
            {
                Pice neki = (Pice)dict[k];
                cmbPice.Items.Add(neki.Naziv);
            }
        }
        
        private void btnBrisi_Click(object sender, EventArgs e)//Brisanje pića iz txt datoteke
        {
            if (cmbBrojevi.Text == "")
            {
                MessageBox.Show("Nista nista odabrali.");
                return;
            }
            else
            {
                int k = int.Parse(cmbBrojevi.Text);
                dict.Remove(k);
                Ispis();
                using (StreamWriter sw = File.CreateText("Pica.txt"))
                {

                    string linija = "";
                    foreach (int ii in dict.Keys)
                    {
                        Pice p = (Pice)dict[ii];
                        linija = p.Naziv + ";" + p.Cijena;
                        sw.WriteLine(linija);
                    }

                }
            }
        }

        private void btnBrisiSve_Click(object sender, EventArgs e)//Brisanje svih stavki iz txt datoteke
        {
            dict.Clear();
            Ispis();
            using (StreamWriter sw = File.CreateText("Pica.txt"))
            {

                string linija = "";
                foreach (int ii in dict.Keys)
                {
                    Pice p = (Pice)dict[ii];
                    linija = p.Naziv + ";" + p.Cijena;
                    sw.WriteLine(linija);
                }

            }
        }

        private void btnUcitaj_Click(object sender, EventArgs e)//učitavanje pića u cmb te ažuriranje popisa pića
        {
            
            dict.Clear();

            string imed = "Pica.txt";
            if (!File.Exists(imed))
            {
                MessageBox.Show("Datoteka ne postoji.");
                return;
            }

            using (StreamReader sr = File.OpenText(imed))
            {
                string linija;
                while ((linija = sr.ReadLine()) != null)
                {
                    string[] rijec = linija.Split(';');
                    string naziv = rijec[0];
                    double cijena = double.Parse(rijec[1]);
                    int id;
                    Random r = new Random();
                    id = r.Next(100, 1000);

                    while (dict.ContainsKey(id))
                        id = r.Next(1, 100);

                    Pice novi = new Pice(naziv,cijena,id);
                    dict.Add(id, novi);
                    

                }
            }
            Ispis();
            Ispis2();
        }

        private void btnKraj_Click(object sender, EventArgs e)
        {
            Application.Exit();//Kraj
        }
        private void btnDodaj_Click(object sender, EventArgs e)//Dodavanje pića u txt datoteku
        {
            int i;

            if(textBox1.Text=="")
            {
                MessageBox.Show("Niste unjeli naziv pića!");
                return;
            }
            if(textBox2.Text=="")
            {
                MessageBox.Show("Niste unjeli cijenu pića!");
                return;
            }
            else if (!int.TryParse(textBox2.Text, out i))
            {
                MessageBox.Show("Cijena pića mora biti brojčana vrijednost!");
                return;
            }

            string na = textBox1.Text;
            double cij = double.Parse(textBox2.Text);
            int id;
            Random r = new Random();
            id = r.Next(100, 1000);//Dodavanje random ID

            while (dict.ContainsKey(id))
                id = r.Next(1, 100);
            Pice novi =new Pice(na, cij, id);
            dict.Add(id,novi);
            using (StreamWriter sw=File.CreateText("Pica.txt"))
            {
                
                string linija = "";
                foreach(int ii in dict.Keys)
                {
                    Pice p = (Pice)dict[ii];
                    linija = p.Naziv + ";" + p.Cijena;
                    sw.WriteLine(linija);
                }
                
            }
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void Form2_Load(object sender, EventArgs e)//izvršavanje prilikom učitavanja forme
        {
            for(int i=1;i<=10;i++)
            {
                cmbKolicina.Items.Add(i);
            }
            label7.Hide();
            label9.Hide();
            cmbPlacanje.Items.Add("GOTOVINA");
            cmbPlacanje.Items.Add("KARTICA");
        }

        private void btnUcitaj2_Click(object sender, EventArgs e)//učitavanje pića u cmb te ažuriranje popisa pića
        {
            dict.Clear();
            iznos_racuna.Clear();
            dnevni_promet.Clear();

            string imed = "Pica.txt";
            if (!File.Exists(imed))
            {
                MessageBox.Show("Datoteka ne postoji.");
                return;
            }

            using (StreamReader sr = File.OpenText(imed))
            {
                string linija;
                while ((linija = sr.ReadLine()) != null)
                {
                    string[] rijec = linija.Split(';');
                    string naziv = rijec[0];
                    double cijena = double.Parse(rijec[1]);
                    int id;
                    Random r = new Random();
                    id = r.Next(100, 1000);

                    while (dict.ContainsKey(id))
                        id = r.Next(1, 100);

                    Pice novi = new Pice(naziv, cijena, id);
                    dict.Add(id, novi);


                }
            }
            Ispis2();
        }

        private void button5_Click(object sender, EventArgs e)//dodavanje pića te količine na račun
        {
            
            if(cmbKolicina.Text=="")
            {
                MessageBox.Show("Niste odabrali količinu!");
                return;
            }
            if(cmbPice.Text=="")
            {
                MessageBox.Show("Niste odabrali piće!");
                return;
            }
            string naziv_pica = cmbPice.SelectedItem.ToString();
            string kolicina_pica = cmbKolicina.SelectedItem.ToString();
            string cijena;
            double ukupni_racun;
            foreach (int k in dict.Keys)
            {
                
                Pice neki = (Pice)dict[k];
                if(neki.Naziv==naziv_pica)
                {
                    cijena = neki.Cijena.ToString();
                    rtbRacun.AppendText(naziv_pica + "\t" + kolicina_pica + "\t" + cijena + "\n");
                    ukupni_racun = double.Parse(kolicina_pica) * double.Parse(cijena);
                    iznos_racuna.Add(ukupni_racun);
                    dnevni_promet.Add(ukupni_racun);
                    
                }
            }
            double b = 0;
            foreach(double a in iznos_racuna)//zbrajanje stavki sa računa
            {
                b = b + a;
            }
            label9.Text = b.ToString()+" KN";
            cmbPice.Text = "";
            cmbKolicina.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)//Kraj unosa pića na račun
        {
            
            if(cmbPlacanje.Text=="")
            {
                MessageBox.Show("Niste odabrali način plaćanja!");
                return;
            }
            else
            {
                rtbRacun.AppendText(DateTime.Now.ToString()+"\n");
                rtbRacun.AppendText("Način plaćanja : "+cmbPlacanje.SelectedItem.ToString());
                label7.Show();
                label9.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();//Kraj
        }
        private void button2_Click(object sender, EventArgs e)//Nova narudžba
        {
            rtbRacun.Clear();
            iznos_racuna.Clear();
            label7.Hide();
            label9.Hide();
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("                    CAFFE BAR ENTER                                ");
            rtbRacun.AppendText("*****************************************************\n");
            rtbRacun.AppendText("NAZIV" + "/" + "KOLIČINA" + "/" + "CIJENA" + "\n");
        }

        private void button3_Click(object sender, EventArgs e)//Obračun dnevnog prometa
        {
            label9.Hide();
            label7.Hide();
            double ukupno=0;
            foreach(double i in dnevni_promet)
            {
                ukupno = ukupno + i;
            }
            rtbRacun.Clear();
            rtbRacun.AppendText("UKUPNI DNEVNI PROMET: " + ukupno.ToString()+" kn");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
