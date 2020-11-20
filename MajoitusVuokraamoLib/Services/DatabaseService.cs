using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo.Services
{
    public interface DatabaseService<T>
    {
        Task<bool> Create(string sql, DynamicParameters parameters);
        Task<List<T>> Read(string sql, DynamicParameters parameters);
        Task<bool> Update(string sql, DynamicParameters parameters);
        Task<bool> Delete(string sql, DynamicParameters parameters);
    }
}
