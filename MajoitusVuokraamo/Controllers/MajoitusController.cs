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
    public static class MajoitusController
    {
        private static MajoitusService majoitusService = new MajoitusService();

        public static bool lisaaMajoitus
            (string paikkakunta, int hinta, int pintaAla, int huoneet, int vuodepaikat, int rakennusvuosi, string lisatiedot, int omistajaId)
        {
            string sql = "INSERT INTO Majoitus (Paikkakunta, Hinta, Huoneet, PintaAla, Rakennusvuosi, Vuodepaikat, Lisatiedot, OmistajaId) VALUES (@Paikkakunta, @Hinta, @Huoneet, @PintaAla, @Rakennusvuosi, @Vuodepaikat, @Lisatiedot, @OmistajaId);";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    { "@Paikkakunta", paikkakunta },
                    { "@Hinta", hinta },
                    { "@Huoneet", huoneet },
                    { "@PintaAla", pintaAla },
                    {  "@Rakennusvuosi", rakennusvuosi },
                    {  "@Vuodepaikat", vuodepaikat },
                    {  "@Lisatiedot", lisatiedot },
                    {  "@OmistajaId", omistajaId }
                };
            var parameters = new DynamicParameters(dictionary);
            return majoitusService.Create(sql, parameters).Result;
        }

        public static DataTable haeMajoitusta
            (string paikkakunta, int? alinHinta, int? ylinHinta, string huoneet, string vuodepaikat, string rakennusvuosi)
        {
            string sql = "SELECT * FROM Majoitus ";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            bool whereClauseAdded = false;
            if (!string.IsNullOrEmpty(paikkakunta))
            {
                sql += " WHERE Paikkakunta=@Paikkakunta ";
                dictionary.Add("@Paikkakunta", paikkakunta);
                whereClauseAdded = true;
            }

            if (!string.IsNullOrEmpty(huoneet))
            {
                if (huoneet.Contains(">"))
                {
                    if (whereClauseAdded)
                        sql += " AND Huoneet>@Huoneet ";
                    else
                    {
                        sql += " WHERE Huoneet>@Huoneet ";
                        whereClauseAdded = true;
                    }
                    dictionary.Add("@Huoneet", huoneet.Substring(1));
                }
                else
                {
                    if (whereClauseAdded)
                        sql += " AND Huoneet=@Huoneet ";
                    else
                    {
                        sql += " WHERE Huoneet=@Huoneet ";
                        whereClauseAdded = true;
                    }
                    dictionary.Add("@Huoneet", huoneet);
                }
            }

            if (!string.IsNullOrEmpty(vuodepaikat))
            {
                if (vuodepaikat.Contains(">"))
                {
                    if (whereClauseAdded)
                        sql += " AND Vuodepaikat>@Vuodepaikat ";
                    else
                    {
                        sql += " WHERE Vuodepaikat>@Vuodepaikat ";
                        whereClauseAdded = true;
                    }
                    dictionary.Add("@Vuodepaikat", vuodepaikat.Substring(1));
                }
                else
                {
                    if (whereClauseAdded)
                        sql += " AND Vuodepaikat=@Vuodepaikat ";
                    else
                    {
                        sql += " WHERE Vuodepaikat=@Vuodepaikat ";
                        whereClauseAdded = true;
                    }
                    dictionary.Add("@Vuodepaikat", vuodepaikat);
                }
            }

            if (!string.IsNullOrEmpty(rakennusvuosi))
            {
                string[] splitted = rakennusvuosi.Split('-');
                string alku = splitted[0].Trim();
                string loppu = splitted[splitted.Length - 1].Trim();

                if (whereClauseAdded)
                    sql += " AND Rakennusvuosi>=@Alku AND Rakennusvuosi<=@Loppu ";
                else
                {
                    sql += " WHERE Rakennusvuosi>=@Alku AND Rakennusvuosi<=@Loppu ";
                    whereClauseAdded = true;
                }
                dictionary.Add("@Alku", alku);
                dictionary.Add("@Loppu", loppu);
            }
            
            if(alinHinta != null)
            {
                if (whereClauseAdded)
                    sql += " AND Hinta>=@AlinHinta";
                else
                {
                    sql += " WHERE Hinta>=@AlinHinta ";
                    whereClauseAdded = true;
                }
                dictionary.Add("@AlinHinta", alinHinta.Value);
            }
            if (ylinHinta != null)
            {
                if (whereClauseAdded)
                    sql += " AND Hinta<=@YlinHinta";
                else
                {
                    sql += " WHERE Hinta<=@YlinHinta ";
                    whereClauseAdded = true;
                }
                dictionary.Add("@YlinHinta", ylinHinta.Value);
            }

            var parameters = new DynamicParameters(dictionary);
            List<Majoitus> majoitukset = majoitusService.Read(sql, parameters).Result;
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Paikkakunta", typeof(string));
            table.Columns.Add("Hinta", typeof(int));
            table.Columns.Add("Huoneet", typeof(int));
            table.Columns.Add("Vuodepaikat", typeof(int));
            table.Columns.Add("Pinta-ala", typeof(int));
            table.Columns.Add("Rakennusvuosi", typeof(int));
            table.Columns.Add("Lisätiedot", typeof(string));
            foreach (Majoitus m in majoitukset)
                table.Rows.Add
                    (m.getId(), m.getPaikkakunta(), m.getHinta(), m.getHuoneet(), m.getVuodepaikat(), m.getPintaAla(), m.getRakennusvuosi(), m.getLisatiedot());

            return table;
        }

        public static Majoitus haeMajoitus(int majoitusId)
        {
            string sql = "SELECT * FROM Majoitus WHERE Id=@MajoitusId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@MajoitusId", majoitusId }
            };
            var parameters = new DynamicParameters(dictionary);
            Majoitus majoitus = majoitusService.Read(sql, parameters).Result.FirstOrDefault();
            return majoitus;
        }

        public static DataTable haeKayttajanMajoitukset(Kayttaja kayttaja)
        {
            string sql = "SELECT * FROM Majoitus WHERE OmistajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttaja.getId()}
            };
            var parameters = new DynamicParameters(dictionary);

            List<Majoitus> majoitukset = majoitusService.Read(sql, parameters).Result;
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Paikkakunta", typeof(string));
            table.Columns.Add("Hinta", typeof(int));
            table.Columns.Add("Huoneet", typeof(int));
            table.Columns.Add("Vuodepaikat", typeof(int));
            table.Columns.Add("PintaAla", typeof(int));
            table.Columns.Add("Rakennusvuosi", typeof(int));
            table.Columns.Add("Lisatiedot", typeof(string));
            foreach (Majoitus m in majoitukset)
                table.Rows.Add
                    (m.getId(), m.getPaikkakunta(), m.getHinta(), m.getHuoneet(), m.getVuodepaikat(), m.getPintaAla(), m. getRakennusvuosi(), m.getLisatiedot());

            return table;
        }

        public static bool poistaKohde(int majoitusId)
        {
            string sql = "DELETE FROM Majoitus WHERE Id=@Id;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Id", majoitusId}
            };
            var parameters = new DynamicParameters(dictionary);

            bool succesful = majoitusService.Delete(sql, parameters).Result;
            if(succesful)
            {
                VarausController.poistaVaraukset(majoitusId);
                return true;
            }
            return false;
        }

        public static bool muokkaaMajoitusta
            (int majoitusId, string paikkakunta, int hinta, int pintaAla, int huoneet, int vuodepaikat, int rakennusvuosi, string lisatiedot)
        {
            string sql = "UPDATE Majoitus SET Paikkakunta=@Paikkakunta, Hinta=@Hinta, Huoneet=@Huoneet, PintaAla=@PintaAla, Vuodepaikat=@Vuodepaikat, Rakennusvuosi=@Rakennusvuosi, Lisatiedot=@Lisatiedot WHERE Id=@Id;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Paikkakunta", paikkakunta },
                { "@Hinta", hinta },
                { "@Huoneet", huoneet },
                {  "@PintaAla", pintaAla },
                { "@Vuodepaikat", vuodepaikat },
                { "@Rakennusvuosi", rakennusvuosi },
                { "@Lisatiedot", lisatiedot },
                { "@Id", majoitusId }
            };
            var parameters = new DynamicParameters(dictionary);
            return majoitusService.Update(sql, parameters).Result;
        }

        public static int laskeMajoitukset()
        {
            string sql = "SELECT Count(*) FROM Majoitus;";
            return majoitusService.Count(sql, null).Result;
        }

        public static int laskeKayttajanMajoitukset(int kayttajaId)
        {
            string sql = "SELECT Count(*) FROM Majoitus WHERE OmistajaId=@KayttajaId;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@KayttajaId", kayttajaId }
            };
            var parameters = new DynamicParameters(dictionary);
            return majoitusService.Count(sql, parameters).Result;
        }
    }
}
