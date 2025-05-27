using System.Data;

namespace Application.Data;
public interface IDapperDbContext
{
    IDbConnection CreateConnection();
}