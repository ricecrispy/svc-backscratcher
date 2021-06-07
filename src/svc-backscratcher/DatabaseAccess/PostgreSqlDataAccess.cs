using Npgsql;
using svc_backscratcher.FluentMap.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DatabaseAccess
{
    public class PostgreSqlDataAccess : IDatabaseAccess
    {
        static PostgreSqlDataAccess()
        {
            FluentMapInitializer.EnsureMapsInitialized();
        }

        public IDbConnection GetDatabaseConnection()
        {
            return new NpgsqlConnection(CreateConnectionString());
        }

        private string CreateConnectionString()
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 5432,
                Username = "postgres",
                Password = "*DwLo8#FiSitaq8R#jUP",
                Database = "backscratcher"
            }.ConnectionString;
        }
    }
}
