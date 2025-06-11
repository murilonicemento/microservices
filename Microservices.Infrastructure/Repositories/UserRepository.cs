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

        return user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        return new ApplicationUser
        {
            UserId = Guid.NewGuid(),
            Email = email,
            Password = password,
            PersonName = "Person name",
            Gender = nameof(GenderOptions.Male)
        };
    }
}