using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Entities
{
    public class Majoitus
    {
        private int id;
        private string paikkakunta;
        private int hinta;
        private int huoneet;
        private int pintaAla;
        private int rakennusvuosi;
        private int vuodepaikat;
        private string lisatiedot;
        private List<Arvostelu> arvostelut;
        private List<Varaus> varaukset;
        private Kayttaja omistaja;

        public Majoitus()
        {

        }

        public Majoitus(string pk, int hi, int hu, int pa, int rv, int vp, string lt)
        {
            paikkakunta = pk;
            hinta = hi;
            huoneet = hu;
            pintaAla = pa;
            rakennusvuosi = rv;
            vuodepaikat = vp;
            lisatiedot = lt;
            varaukset = new List<Varaus>();
        }



        public int getId()
        {
            return id;
        }

        public string getPaikkakunta()
        {
            return paikkakunta;
        }

        public void setPaikkakunta(string pk)
        {
            paikkakunta = pk;
        }

        public int getHinta()
        {
            return hinta;
        }

        public void setHinta(int hi)
        {
            hinta = hi;
        }

        public int getHuoneet()
        {
            return huoneet;
        }

        public void setHuoneet(int hu)
        {
            huoneet = hu;
        }

        public int getPintaAla()
        {
            return pintaAla;
        }

        public void setPintaAla(int pa)
        {
            pintaAla = pa;
        }

        public int getRakennusvuosi()
        {
            return rakennusvuosi;
        }

        public void setRakennusvuosi(int rv)
        {
            rakennusvuosi = rv;
        }

        public int getVuodepaikat()
        {
            return vuodepaikat;
        }

        public void setVuodepaikat(int vp)
        {
            vuodepaikat = vp;
        }

        public string getLisatiedot()
        {
            return lisatiedot;
        }

        public void setLisatiedot(string lt)
        {
            lisatiedot = lt;
        }

        public List<Varaus> getVaraukset()
        {
            return varaukset;
        }

        public void setVaraukset(List<Varaus> v)
        {
            varaukset = v;
        }

        public void addVaraus(Varaus v)
        {
            varaukset.Add(v);
        }

        public List<Arvostelu> getArvostelut()
        {
            return arvostelut;
        }

        public void addArvostelu(Arvostelu a)
        {
            arvostelut.Add(a);
        }

        public Kayttaja getOmistaja()
        {
            return omistaja;
        }

        public override string ToString()
        {
            double arvio = LaskeArvio();
            return $"Paikkakunta: {paikkakunta} | Hinta: {hinta} €/vrk | Arvio: {Math.Round(LaskeArvio(), 1)}";
        }

        private double LaskeArvio()
        {
            double summa = 0.0;
            if (arvostelut.Count == 0)
                return summa;
            foreach (Arvostelu a in arvostelut)
                summa += (int)a.getArvio();

            return summa / arvostelut.Count;
        }
    }
}
