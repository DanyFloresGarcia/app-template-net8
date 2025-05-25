using System.Data;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Application.Data;

public class OracleDapperDbContext : IDapperDbContext
{
    private readonly string _connectionString;

    public OracleDapperDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new OracleConnection(_connectionString);
    }
}
