using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Entities
{
    public class Varaus
    {
        private int id;
        private string alkuAika;
        private string loppuAika;
        private int kayttajaId;
        private int majoitusId;

        public Varaus()
        {

        }

        public Varaus(string aa, string la)
        {
            alkuAika = aa;
            loppuAika = la;
        }

        public int getId()
        {
            return id;
        }

        public string getAlkuAika()
        {
            return alkuAika;
        }

        public void setAlkuAika(string aa)
        {
            alkuAika = aa;
        }

        public string getLoppuAika()
        {
            return loppuAika;
        }

        public void setLoppuAika(string la)
        {
            loppuAika = la;
        }

        public int getMajoitusId()
        {
            return majoitusId;
        }

        public DateTime varausAlkaaDateTime()
        {
            string[] split = alkuAika.Split('.');
            return new DateTime(int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[0]));
        }

        public DateTime varausLoppuuDateTime()
        {
            string[] split = loppuAika.Split('.');
            return new DateTime(int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[0]));
        }
    }
}
