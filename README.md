![image](https://github.com/s28468/task12/assets/161838169/ee31262a-7f52-45be-92c1-09637523e39f)

DELETE endpoint?

## Installation

```
git clone https://github.com/s28468/task12
```

```
cd WebApplication1
```

Check your connection string in 'appsettings.json'
  ```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_address;Database=your_database_name;User Id=your_username;Password=your_password;"
  },
  "JwtSettings": {
    "Key": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  }
}
  ```
- Check all dependencies

## Features
 - Register a new user 
 - Login user and get tokens
- Refresh access token using refresh token 
## Endpoints
```POST /api/auth/register``` - Registers a new user.

```POST /api/auth/login``` - Authenticates a user and returns access and refresh tokens.

```POST /api/auth/refresh``` - Refreshes the access token using the refresh token.
## Dependencies
The project uses the following dependencies:

- ASP.NET Core 5.0 or later
 - Entity Framework Core
- SQL Server (or another compatible database)
- NuGet Packages
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.IdentityModel.Tokens
- Microsoft.OpenApi
