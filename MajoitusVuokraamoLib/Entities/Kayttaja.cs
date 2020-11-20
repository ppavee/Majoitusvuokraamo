using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Entities
{
    public class Kayttaja
    {
        private int id;
        private string etunimi;
        private string sukunimi;
        private string salasana;
        private string puhelinnumero;
        private string sahkoposti;
        private List<Arvostelu> arvostelut;
        private List<Varaus> varaukset;

        public Kayttaja()
        {

        }

        public Kayttaja(string en, string sn, string ss, string pn, string sp)
        {
            etunimi = en;
            sukunimi = sn;
            salasana = ss;
            puhelinnumero = pn;
            sahkoposti = sp;
            arvostelut = new List<Arvostelu>();
            varaukset = new List<Varaus>();
        }

        public Kayttaja(int kayttajaId, string en, string sn, string ss, string pn, string sp)
        {
            id = kayttajaId;
            etunimi = en;
            sukunimi = sn;
            salasana = ss;
            puhelinnumero = pn;
            sahkoposti = sp;
            arvostelut = new List<Arvostelu>();
            varaukset = new List<Varaus>();
        }

        public int getId()
        {
            return id;
        }

        public string getEtunimi()
        {
            return etunimi;
        }

        public void setEtunimi(string en)
        {
            etunimi = en;
        }

        public string getSukunimi()
        {
            return sukunimi;
        }

        public void setSukunimi(string sn)
        {
            sukunimi = sn;
        }

        public string getSalasana()
        {
            return salasana;
        }

        public void setSalasana(string ss)
        {
            salasana = ss;
        }

        public string getPuhelinnumero()
        {
            return puhelinnumero;
        }

        public void setPuhelinnumero(string pn)
        {
            puhelinnumero = pn;
        }

        public string getSahkoposti()
        {
            return sahkoposti;
        }

        public void setSahkoposti(string sp)
        {
            sahkoposti = sp;
        }

        public List<Varaus> getVaraukset()
        {
            return varaukset;
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
    }
}
