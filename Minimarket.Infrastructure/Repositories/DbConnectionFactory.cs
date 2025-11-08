using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Minimarket.Core.Enum;
using Minimarket.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Minimarket.Infrastructure.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _sqlConn;
        private readonly string _mySqlConn;
        private readonly IConfiguration _config;
        public DatabaseProvider Provider { get; }

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
            _sqlConn = _config.GetConnectionString("ConnectionSqlServer") ?? string.Empty;
            _mySqlConn = _config.GetConnectionString("ConnectionMySql") ?? string.Empty;

            var providerStr = _config.GetSection("DatabaseProvider").Value
            ?? "SqlServer";

            Provider = providerStr.Equals("MySql", StringComparison.OrdinalIgnoreCase)
            ? DatabaseProvider.MySql
            : DatabaseProvider.SqlServer;
        }
        public IDbConnection CreateConnection()
        {
            return Provider switch
            {
                DatabaseProvider.MySql => new MySqlConnection(_mySqlConn),
                _ => new SqlConnection(_sqlConn),
            };
        }

    }
}
