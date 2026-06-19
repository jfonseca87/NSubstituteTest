using System.Data;

namespace NSubstituteTest.Data;

public interface IConnectionFactory
{
    IDbConnection CreateConnection();
}
