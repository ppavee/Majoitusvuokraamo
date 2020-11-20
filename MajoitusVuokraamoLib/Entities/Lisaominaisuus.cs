using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Entities
{
    public class Lisaominaisuus
    {
        private int id;
        private string nimi;
        private int majoitusId;

        public Lisaominaisuus()
        {

        }

        public Lisaominaisuus(string n)
        {
            nimi = n;
        }

        public int getId()
        {
            return id;
        }

        public string getNimi()
        {
            return nimi;
        }

        public void setNimi(string n)
        {
            nimi = n;
        }

        public int getMajoitusId()
        {
            return majoitusId;
        }
    }

}
