using Dapper;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamoLib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Controllers
{
    public static class VarausController
    {
        private static VarausService varausService = new VarausService();

        public static List<Varaus> haeVoimassaOlevatVaraukset(int majoitusId)
        {
            string sql = "SELECT * FROM Varaus WHERE MajoitusId=@MajoitusId";
            var parameters = new DynamicParameters();
            parameters.Add("@MajoitusId", majoitusId);
            List<Varaus> varaukset = varausService.Read(sql, parameters).Result;
            return varaukset;
        }

        public static bool varaaMajoitus(Kayttaja varaaja, int majoitusId, DateTime alkaa, DateTime loppuu)
        {
            string alkaaString = $"{alkaa.Day}.{alkaa.Month}.{alkaa.Year}";
            string loppuuString = $"{loppuu.Day}.{loppuu.Month}.{loppuu.Year}";
            string sql = "INSERT INTO Varaus (AlkuAika, LoppuAika, MajoitusId, KayttajaId) VALUES (@AlkuAika, @LoppuAika, @MajoitusId, @KayttajaId);";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@AlkuAika", alkaaString },
                { "@LoppuAika", loppuuString },
                { "@MajoitusId", majoitusId},
                { "@KayttajaId", varaaja.getId()}
            };
            var parameters = new DynamicParameters(dictionary);
            return varausService.Create(sql, parameters).Result;
        }

        public static bool poistaVaraus(int varausId)
        {
            string sql = "DELETE FROM Varaus WHERE Id=@VarausId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@VarausId", varausId }
            };
            var parameters = new DynamicParameters(dictionary);
            return varausService.Delete(sql, parameters).Result;
        }

        public static bool poistaVaraukset(int majoitusId)
        {
            string sql = "DELETE FROM Varaus WHERE MajoitusId=@MajoitusId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@MajoitusId", majoitusId }
            };
            var parameters = new DynamicParameters(dictionary);
            return varausService.Delete(sql, parameters).Result;
        }

        public static DataTable haeKayttajanVaraukset(Kayttaja kayttaja)
        {
            string sql = "SELECT * FROM Varaus WHERE KayttajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttaja.getId()}
            };
            var parameters = new DynamicParameters(dictionary);

            List<Varaus> varaukset = varausService.Read(sql, parameters).Result;
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("alkuAika", typeof(DateTime));
            table.Columns.Add("loppuAika", typeof(DateTime));
            table.Columns.Add("majoitusId", typeof(int));
            foreach (Varaus v in varaukset)
                table.Rows.Add(v.getId(), v.varausAlkaaDateTime(), v.varausLoppuuDateTime(), v.getMajoitusId());

            return table;
        }

        public static DataTable haeKayttajanVoimassaOlevatVaraukset(Kayttaja kayttaja)
        {

            string sql = "SELECT * FROM Varaus WHERE KayttajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttaja.getId()}
            };
            var parameters = new DynamicParameters(dictionary);

            List<Varaus> varaukset = varausService.Read(sql, parameters).Result;
            DataTable table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("alkuAika", typeof(DateTime));
            table.Columns.Add("loppuAika", typeof(DateTime));
            table.Columns.Add("majoitusId", typeof(int));
            foreach (Varaus v in varaukset.Where(_ =>_.varausLoppuuDateTime().AddDays(1) >= DateTime.Now))
                table.Rows.Add(v.getId(), v.varausAlkaaDateTime(), v.varausLoppuuDateTime(), v.getMajoitusId());

            return table;
        }

        public static int laskeVaraukset()
        {
            string sql = "SELECT Count(*) FROM Varaus;";
            return varausService.Count(sql, null).Result;
        }

        public static int laskeKayttajanVaraukset(int kayttajaId)
        {
            string sql = "SELECT Count(*) FROM Varaus WHERE KayttajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttajaId }
            };
            var parameters = new DynamicParameters(dictionary);
            return varausService.Count(sql, parameters).Result;
        }
    }
}
