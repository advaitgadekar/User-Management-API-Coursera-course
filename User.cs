namespace UserManagementApi.Models;

public sealed record User(int Id, string Name, string Email, int Age);

public sealed record CreateUserRequest(string? Name, string? Email, int Age);

public sealed record UpdateUserRequest(string? Name, string? Email, int Age);
