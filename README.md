# Overview

ASP.NET API for spellbound. Swagger has been included for documentation [Swagger Documentation](https://localhost:5001/swagger)

## Database

Using EF Core with SQLite.
Use the following to create database migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- [EF Core Tutorial](https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db?tabs=netcore-cli)

## Secret storage

Using [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=macos) for setting secrets in dev environment.

``` bash
dotnet user-secrets set "MongoDb:ConnectionString" "some_random_value" --project spellbound-api/spellbound-api.csproj
```

## Identity Management

Using ASP.NET Core Identity and JwtToken to manage authentication and authorization. See tutorials:

- [ASP.NET Core Ideneity and Tokens](https://medium.com/@ozgurgul/asp-net-core-2-0-webapi-jwt-authentication-with-identity-mysql-3698eeba6ff8)
- [ASP.NET Core Tokens](https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/)

## Unit Tests

Using xUNit, see tutorials:

- [C# unit testing with xunit](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- [Testing controllers](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-2.1)

## Code generation

```bash
dotnet aspnet-codegenerator controller -name TestController -m Spell -dc ApplicationDbConext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```

## Documentation 

Using Swagger UI (Swashbuckle) [Swagger UI](https://localhost:5001/swagger).

- [Tutorial setting up token auth](https://ppolyzos.com/2017/10/30/add-jwt-bearer-authorization-to-swagger-and-asp-net-core/)

## TODO

- Update CORS policy
- Add rest of the Spells API from spellbound project
- Add User API
- Add Character API
- Unit tests
