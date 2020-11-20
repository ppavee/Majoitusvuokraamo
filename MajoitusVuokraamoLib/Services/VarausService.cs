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
    public class VarausService : DatabaseService<Varaus>
    {

        public async Task<bool> Create(string sql, DynamicParameters parameters)
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
                // Log Errors in the future?
                return false;
            }
        }

        public async Task<List<Varaus>> Read(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            List<Varaus> result = new List<Varaus>();
            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<Varaus>(sql, parameters)).ToList();
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
