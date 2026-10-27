using System.Collections.Concurrent;
using System.Text;
using UserManagementApi.Models;
using UserManagementApi.Middleware;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserService, InMemoryUserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Demo API-key middleware protects write operations. Set API_KEY in the environment.
// GET endpoints remain public so the API can be explored easily.
app.UseWhen(
    context => context.Request.Method is "POST" or "PUT" or "DELETE",
    branch => branch.UseMiddleware<ApiKeyMiddleware>());

app.MapGet("/", () => Results.Ok(new
{
    name = "User Management API",
    version = "1.0",
    endpoints = new[] { "GET /api/users", "GET /api/users/{id}", "POST /api/users", "PUT /api/users/{id}", "DELETE /api/users/{id}" },
    documentation = "/swagger"
}));

app.MapGet("/api/users", (IUserService service) =>
    Results.Ok(service.GetAll()));

app.MapGet("/api/users/{id:int}", (int id, IUserService service) =>
{
    var user = service.GetById(id);
    return user is null ? Results.NotFound(new { message = $"User with id {id} was not found." }) : Results.Ok(user);
});

app.MapPost("/api/users", (CreateUserRequest request, IUserService service) =>
{
    var errors = UserValidation.Validate(request.Name, request.Email, request.Age);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    if (service.EmailExists(request.Email))
        return Results.Conflict(new { message = "A user with this email already exists." });

    var created = service.Create(request);
    return Results.Created($"/api/users/{created.Id}", created);
});

app.MapPut("/api/users/{id:int}", (int id, UpdateUserRequest request, IUserService service) =>
{
    var errors = UserValidation.Validate(request.Name, request.Email, request.Age);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);

    if (service.EmailExists(request.Email, id))
        return Results.Conflict(new { message = "A different user with this email already exists." });

    var updated = service.Update(id, request);
    return updated is null ? Results.NotFound(new { message = $"User with id {id} was not found." }) : Results.Ok(updated);
});

app.MapDelete("/api/users/{id:int}", (int id, IUserService service) =>
    service.Delete(id)
        ? Results.NoContent()
        : Results.NotFound(new { message = $"User with id {id} was not found." }));

app.Run();
