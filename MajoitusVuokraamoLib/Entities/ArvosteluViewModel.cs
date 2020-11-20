using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamoLib.Entities
{
    public class ArvosteluViewModel
    {
        public int Arvio { get; set; }
        public string Kommentti { get; set; }
        public string Aika { get; set; }
        public string  Etunimi { get; set; }
        public string Sukunimi { get; set; }
    }
}
