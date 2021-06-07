using Microsoft.Extensions.Options;
using Npgsql;
using svc_backscratcher.FluentMap.Mappings;
using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DatabaseAccess
{
    public class PostgreSqlDataAccess : IDatabaseAccess
    {
        private readonly PostgreSqlDataAccessOptions _options;

        public PostgreSqlDataAccess(IOptions<PostgreSqlDataAccessOptions> options)
        {
            _options = options?.Value;
        }

        static PostgreSqlDataAccess()
        {
            FluentMapInitializer.EnsureMapsInitialized();
        }

        public IDbConnection GetDatabaseConnection()
        {
            //NpgsqlConnection.GlobalTypeMapper.MapEnum<BackScratcherSize>("data.size");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<BackScratcherSize>("public.size");
            return new NpgsqlConnection(CreateConnectionString());
        }

        private string CreateConnectionString()
        {
            return new NpgsqlConnectionStringBuilder
            {
                //Host = "localhost",
                Host = _options.Host,
                Port = _options.Port,
                Username = _options.Username,
                Password = _options.Password,
                Database = _options.Database
            }.ConnectionString;
        }
    }
}
