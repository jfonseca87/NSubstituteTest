using Microsoft.Data.SqlClient;
using System.Data;

namespace NSubstituteTest.Data;

public class SqlServerConnectionFactory(string connectionString) : IConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection()
    {
        var conn = new SqlConnection(_connectionString);
        return conn;
    }
}
