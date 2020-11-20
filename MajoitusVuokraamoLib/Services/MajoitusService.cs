using Dapper;
using MajoitusVuokraamo;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamo.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamoLib.Services
{
    public class MajoitusService : DatabaseService<Majoitus>
    {
        private VarausService varausService = new VarausService();
        public async Task<bool> Create(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            try
            {
                //int k;
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    await conn.ExecuteAsync(sql, parameters);
                }
                return true;
            }
            catch(Exception e)
            {
                // Log Errors in the future?
                return false;
            }
        }
      

        public async Task<List<Majoitus>> Read(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            
            List<Majoitus> result = new List<Majoitus>();
            string sqlVaraukset = "SELECT * FROM Varaus WHERE MajoitusId IN (@MajoitusIds);";

            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<Majoitus>(sql, parameters)).ToList();
                    Dictionary<string, object> dictionary = new Dictionary<string, object>
                    {
                        { "@MajoitusIds", string.Join(",", result.Select(_ => _.getId()))}
                    };
                    var parametersVaraukset = new DynamicParameters(dictionary);
                    sqlVaraukset = sqlVaraukset.Replace("@MajoitusIds", string.Join(",", result.Select(_ => _.getId())));
                    List<Varaus> varaukset = await varausService.Read(sqlVaraukset, null);

                    foreach (Majoitus m in result)
                        m.setVaraukset(varaukset.Where(_ => _.getMajoitusId() == m.getId()).ToList());
                }
            }
            catch (Exception e)
            {
                // Log Errors in the future?
            }
            return result;
        }


        public async Task<bool> Delete(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();

            try
            {
                int k;
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    k = await conn.ExecuteAsync(sql, parameters);

                }
                return k >= 0;
            }
            catch (Exception e)
            {
                // Log Errors in the future?
                return false;
            }
        }

        public async Task<bool> Update(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();

            try
            {
                int k;
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    k = await conn.ExecuteAsync(sql, parameters);
                }
                return k >= 0;
            }
            catch (Exception e)
            {
                // Log Errors in the future?
                return false;
            }
        }

        public async Task<int> Count(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();

            try
            {
                int k;
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    k = (await conn.QueryAsync<int>(sql, parameters)).FirstOrDefault();
                }
                return k;
            }
            catch (Exception e)
            {
                // Log Errors in the future?
                return -1;
            }
        }
    }
}
