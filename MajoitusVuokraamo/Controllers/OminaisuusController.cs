using Dapper;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamoLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Controllers
{
    public static class OminaisuusController
    {
        private static OminaisuusService ominaisuusService = new OminaisuusService();
        public static bool lisaaOminaisuudet(List<string> ominaisuudet, int majoitusId)
        {
            string sqlDelete = "DELETE FROM Lisaominaisuus WHERE MajoitusId=@MajoitusId;";
            Dictionary<string, object> dictionary2 = new Dictionary<string, object>
                    {
                        { "@MajoitusId",majoitusId }
                    };
            var parameters2 = new DynamicParameters(dictionary2);
            bool a = ominaisuusService.Delete(sqlDelete, parameters2).Result;

            string sql = "INSERT INTO Lisaominaisuus (Nimi, MajoitusId) VALUES (@Nimi, @MajoitusId);";
            foreach (string ominaisuus in ominaisuudet)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                    {
                        { "@Nimi", ominaisuus },
                        { "@MajoitusId",majoitusId }
                    };
                var parameters = new DynamicParameters(dictionary);
                if (!ominaisuusService.Create(sql, parameters).Result)
                    return false;
            }
            return true;

        }

        public static List<Lisaominaisuus> haeOminaisuudet(int majoitusId)
        {
            string sql = "SELECT * FROM Lisaominaisuus WHERE MajoitusId=@MajoitusId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
                    {
                        { "@MajoitusId",majoitusId }
                    };
            var parameters = new DynamicParameters(dictionary);
            return ominaisuusService.Read(sql, parameters).Result;
        }
}
}
