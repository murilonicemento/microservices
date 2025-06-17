using Microservices.Core.Entities;

namespace Microservices.Core.RepositoriesContracts;

/// <summary>
/// Contract to be implemented by UserRepository that contains data access logic of Users data store
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Method to add a user to the data store and return the added user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task<ApplicationUser?> AddUser(ApplicationUser user);

    /// <summary>
    /// Method to retrieve existing user by their email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
    
    /// <summary>
    /// Returns the users data based on the given user ID
    /// </summary>
    /// <param name="userId">User ID to search</param>
    /// <returns>ApplicationUser object that matches with given UserId</returns>
    public Task<ApplicationUser?> GetUserByUserId(Guid userId);
}