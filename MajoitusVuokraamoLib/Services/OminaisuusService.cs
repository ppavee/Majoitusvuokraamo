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
    public class OminaisuusService : DatabaseService<Lisaominaisuus>
    {

        public async Task<bool> Create(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    var k = await conn.ExecuteAsync(sql, parameters);
                }
                return true;
            }
            catch (Exception e)
            {
                // LOG ERRORS IN FUTURE
                return false;
            }
        }

        public async Task<List<Lisaominaisuus>> Read(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            try
            {
                List<Lisaominaisuus> result = new List<Lisaominaisuus>();
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<Lisaominaisuus>(sql, parameters)).ToList();
                }
                return result;
            }
            catch (Exception e)
            {
                // LOG ERRORS IN FUTURE
                return null;
            }

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
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    await conn.ExecuteAsync(sql, parameters);
                }
                return true;
            }
            catch (Exception e)
            {
                // Log Errors in the future?
                return false;
            }
        }

    }
}
