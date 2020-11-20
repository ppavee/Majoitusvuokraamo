using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Entities
{
    public enum Arvio
    {
        EiArviota = 0,
        AlaArvoinen = 1,
        Huono = 2,
        Tyydyttava = 3,
        Hyva = 4,
        Erinomainen = 5
    }
    public class Arvostelu
    {
        private int id;
        private DateTime aika;
        private string kommentti;
        private Arvio arvio;
        private int kayttajaId;
        private int majoitusId;

        public Arvostelu()
        {

        }

        public Arvostelu(DateTime ai, string k, Arvio ar)
        {
            aika = ai;
            kommentti = k;
            arvio = ar;
        }

        public int getId()
        {
            return id;
        }

        public DateTime getAika()
        {
            return aika;
        }

        public void setAika(DateTime ai)
        {
            aika = ai;
        }

        public string getKommentti()
        {
            return kommentti;
        }

        public void setKommentti(string k)
        {
            kommentti = k;
        }

        public Arvio getArvio()
        {
            return arvio;
        }

        public void setArvio(Arvio ar)
        {
            arvio = ar;
        }
    }
}
