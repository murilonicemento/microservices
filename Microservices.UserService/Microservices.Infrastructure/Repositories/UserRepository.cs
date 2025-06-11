using Dapper;
using Microservices.Core.DTO;
using Microservices.Core.Entities;
using Microservices.Core.RepositoriesContracts;
using Microservices.Infrastructure.DbContext;

namespace Microservices.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly DapperDbContext _dbContext;

    public UserRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();

        const string query =
            "INSERT INTO \"Users\" (\"UserId\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES (@UserId, @Email, @PersonName, @Gender, @Password)";

        var rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);

        return rowCountAffected > 0 ? user : null;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        const string query =
            "SELECT \"UserId\", \"Email\", \"PersonName\", \"Gender\", \"Password\" FROM \"Users\" WHERE \"Email\" = @Email AND \"Password\" = @Password";
        var parameters = new { Email = email, Password = password };

        return await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);
    }
}