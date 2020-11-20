using Dapper;
using MajoitusVuokraamo.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Services
{
    public class KayttajaService : DatabaseService<Kayttaja>
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


        public async Task<Kayttaja> ReadSingle(string username)
        {
            string cs = DatabaseConnection.GetConnectionString();
            string sql = "SELECT * FROM Kayttaja WHERE Sahkoposti = @username;";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("username", username);
            Kayttaja kayttaja = null;
            
            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    kayttaja = (await conn.QueryAsync<Kayttaja>(sql, parameters)).ToList().FirstOrDefault();
                }
            }
            catch(Exception e)
            {

            }
            return kayttaja;
        }

        public async Task<List<Kayttaja>> Read(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            List<Kayttaja> result = new List<Kayttaja>();
            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<Kayttaja>(sql, parameters)).ToList();
                }
            }
            catch (Exception e)
            {
                // Log Errors in the future?
            }
            return result;
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
                return k > 0;
            }
            catch (Exception e)
            {
                // LOG ERRORS IN FUTURE
                return false;
            }
        }

        public async Task<bool> Delete(string sql, DynamicParameters parameters)
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
                // LOG ERRORS IN FUTURE
                return false;
            }
        }

        public async Task<int> Count(string sql)
        {
            string cs = DatabaseConnection.GetConnectionString();

            try
            {
                int k;
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    k = (await conn.QueryAsync<int>(sql)).FirstOrDefault();
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
