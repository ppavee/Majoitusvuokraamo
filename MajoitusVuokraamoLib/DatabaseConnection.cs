using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajoitusVuokraamo
{
    public static class DatabaseConnection
    {

        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteDB"].ConnectionString;
        }
    }
}
