using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace UserService.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;
    public IDbConnection DbConnection => _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("PostgreSQL");
        
        _connection = new NpgsqlConnection(connectionString);
    }
}