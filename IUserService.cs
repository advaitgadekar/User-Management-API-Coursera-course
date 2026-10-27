using UserManagementApi.Models;

namespace UserManagementApi.Services;

public interface IUserService
{
    IReadOnlyCollection<User> GetAll();
    User? GetById(int id);
    User Create(CreateUserRequest request);
    User? Update(int id, UpdateUserRequest request);
    bool Delete(int id);
    bool EmailExists(string? email, int? exceptUserId = null);
}
