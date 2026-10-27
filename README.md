# User Management API

A learning project built with **ASP.NET Core Minimal APIs (.NET 8)**. It demonstrates CRUD operations, input validation, duplicate-email protection, request-logging middleware, and API-key protection for write operations.

## Features

- `GET /api/users` — list all users
- `GET /api/users/{id}` — retrieve one user
- `POST /api/users` — create a user
- `PUT /api/users/{id}` — update a user
- `DELETE /api/users/{id}` — delete a user
- Validation: name length, email format, and age range (13–120)
- Duplicate email addresses return `409 Conflict`
- Request logging middleware records method, path, status, and duration
- API-key middleware protects `POST`, `PUT`, and `DELETE`
- Swagger UI in Development mode

> **Storage note:** This project uses in-memory storage. Records reset when the application restarts. For a production API, use a database and proper identity/authentication.

## Requirements

- .NET 8 SDK
- Git
- A GitHub account

## Run locally

From this directory:

```bash
dotnet restore
dotnet run
```

Open the URL shown in the terminal and append `/swagger` if Swagger is enabled in Development.

### Enable write operations

Set an API key before starting the application.

PowerShell:
```powershell
$env:API_KEY = "replace-with-a-long-random-development-key"
dotnet run
```

Bash:
```bash
export API_KEY="replace-with-a-long-random-development-key"
dotnet run
```

For `POST`, `PUT`, and `DELETE`, include this header:

```http
X-API-Key: replace-with-a-long-random-development-key
```

Do not commit real API keys to GitHub.

## Example requests

List users:
```bash
curl http://localhost:5000/api/users
```

Create user (replace the port with the URL printed by `dotnet run`):
```bash
curl -X POST http://localhost:5000/api/users \
  -H "Content-Type: application/json" \
  -H "X-API-Key: replace-with-a-long-random-development-key" \
  -d '{"name":"Riya Deshmukh","email":"riya@example.com","age":20}'
```

Get user:
```bash
curl http://localhost:5000/api/users/1
```

Update user:
```bash
curl -X PUT http://localhost:5000/api/users/1 \
  -H "Content-Type: application/json" \
  -H "X-API-Key: replace-with-a-long-random-development-key" \
  -d '{"name":"Aarav S.","email":"aarav.updated@example.com","age":22}'
```

Delete user:
```bash
curl -X DELETE http://localhost:5000/api/users/1 \
  -H "X-API-Key: replace-with-a-long-random-development-key"
```

## Test validation

Try a request with an invalid email, a name shorter than two characters, or an age outside 13–120. The API returns `400 Bad Request` with field-level validation errors. Try creating the same email twice; the second request returns `409 Conflict`.

## GitHub submission

1. Create a new **public** GitHub repository named `UserManagementApi` (or use your course's required visibility).
2. Extract this project folder and open a terminal in it.
3. Run:

```bash
git init
git add .
git commit -m "Build user management API with CRUD, validation, and middleware"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/UserManagementApi.git
git push -u origin main
```

Replace `YOUR-USERNAME` with your GitHub username and use the exact URL shown by GitHub.

## Copilot debugging evidence — complete this honestly

I cannot operate GitHub Copilot in your account or truthfully claim it was used. To satisfy the Copilot criterion, open this project in VS Code with GitHub Copilot enabled and ask Copilot Chat:

> Review this ASP.NET Core user management API for compile errors, incorrect HTTP status codes, validation bugs, race conditions, and security issues. Explain each issue and suggest a minimal fix. Do not claim tests passed unless you ran them.

Apply any useful fixes, run `dotnet build`, and save a screenshot of your Copilot conversation plus the successful build output for your submission. Add a short note here describing the real issue Copilot helped you find and how you fixed it.

## Pre-submission checklist

- [ ] Repository created and code pushed to GitHub
- [ ] `dotnet build` succeeds on your machine
- [ ] Tested GET, POST, PUT, and DELETE
- [ ] Tested invalid input and duplicate email handling
- [ ] Used GitHub Copilot and captured genuine evidence
- [ ] Tested logging middleware and API-key protection
