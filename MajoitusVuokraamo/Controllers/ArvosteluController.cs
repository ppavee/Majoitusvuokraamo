using Dapper;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamoLib.Entities;
using MajoitusVuokraamoLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Controllers
{
    public static class ArvosteluController
    {
        private static ArvosteluService arvosteluService = new ArvosteluService();
        public static bool lisaaArvostelu(int? arvio, string kommentti, DateTime aika, int kayttajaId, int majoitusId)
        {
            string sql = "INSERT INTO Arvostelu (Aika, Kommentti, Arvio, MajoitusId, KayttajaId) VALUES (@Aika, @Kommentti, @Arvio, @MajoitusId, @KayttajaId);";
            string aikaS = $"{aika.Day}.{aika.Month}.{aika.Year}";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Aika", aikaS },{ "@Kommentti", kommentti },
                { "@Arvio",arvio },{ "@MajoitusId",majoitusId },
                {  "@KayttajaId", kayttajaId }
            };
            var parameters = new DynamicParameters(dictionary);
            return arvosteluService.Create(sql, parameters).Result;
        }

        public static ArvosteluViewModels haeArvostelut(int majoitusId)
        {
            string sql = "SELECT Arvostelu.Aika, Arvostelu.Kommentti, Arvostelu.Arvio, Kayttaja.Etunimi, Kayttaja.Sukunimi FROM Arvostelu INNER JOIN Kayttaja ON Kayttaja.Id=Arvostelu.KayttajaId WHERE Arvostelu.MajoitusId=@MajoitusId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@MajoitusId",majoitusId },
            };
            var parameters = new DynamicParameters(dictionary);
            return arvosteluService.ReadArvosteluViewModel(sql, parameters).Result;
        }

        public static int laskeArvostelut()
        {
            string sql = "SELECT Count(*) FROM Arvostelu;";
            return arvosteluService.Count(sql, null).Result;
        }

        public static int laskeKayttajanArvostelut(int kayttajaId)
        {
            string sql = "SELECT Count(*) FROM Arvostelu WHERE KayttajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttajaId }
            };
            var parameters = new DynamicParameters(dictionary);
            return arvosteluService.Count(sql, parameters).Result;
        }
    }
}
