using System.Collections.Concurrent;
using UserManagementApi.Models;

namespace UserManagementApi.Services;

// In-memory storage is suitable for this learning assignment.
// Data resets whenever the application restarts.
public sealed class InMemoryUserService : IUserService
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private int _nextId = 0;

    public InMemoryUserService()
    {
        Create(new CreateUserRequest("Aarav Sharma", "aarav@example.com", 21));
        Create(new CreateUserRequest("Maya Patel", "maya@example.com", 22));
    }

    public IReadOnlyCollection<User> GetAll() =>
        _users.Values.OrderBy(user => user.Id).ToArray();

    public User? GetById(int id) =>
        _users.TryGetValue(id, out var user) ? user : null;

    public User Create(CreateUserRequest request)
    {
        var id = Interlocked.Increment(ref _nextId);
        var user = new User(id, request.Name!.Trim(), request.Email!.Trim().ToLowerInvariant(), request.Age);
        _users[id] = user;
        return user;
    }

    public User? Update(int id, UpdateUserRequest request)
    {
        if (!_users.ContainsKey(id))
            return null;

        var updated = new User(id, request.Name!.Trim(), request.Email!.Trim().ToLowerInvariant(), request.Age);
        _users[id] = updated;
        return updated;
    }

    public bool Delete(int id) => _users.TryRemove(id, out _);

    public bool EmailExists(string? email, int? exceptUserId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return _users.Values.Any(user =>
            user.Id != exceptUserId &&
            string.Equals(user.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
