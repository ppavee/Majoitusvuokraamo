using Dapper;
using MajoitusVuokraamo;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamo.Services;
using MajoitusVuokraamoLib.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamoLib.Services
{
    public class ArvosteluService : DatabaseService<Arvostelu>
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

        public async Task<List<Arvostelu>> Read(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            List<Arvostelu> result = new List<Arvostelu>();
            try
            {
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<Arvostelu>(sql, parameters)).ToList();
                }
            }
            catch (Exception e)
            {
                // Log Errors in the future?
            }
            return result;
        }

        public async Task<ArvosteluViewModels> ReadArvosteluViewModel(string sql, DynamicParameters parameters)
        {
            string cs = DatabaseConnection.GetConnectionString();
            try
            {
                List <ArvosteluViewModel> result = new List<ArvosteluViewModel>();
                using (IDbConnection conn = new SQLiteConnection(cs))
                {
                    result = (await conn.QueryAsync<ArvosteluViewModel>(sql, parameters)).ToList();
                }
                ArvosteluViewModels arvostelut = new ArvosteluViewModels(result);

                return arvostelut;
            }
            catch (Exception e)
            {
                // LOG ERRORS IN FUTURE
                return null;
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
    }
}
