# Overview

ASP.NET API for spellbound. Swagger has been included for documentation [Swagger Documentation](https://localhost:5001/swagger)

## Secret storage

Using [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=macos) for setting secrets in dev environment.
Example:

``` bash
dotnet user-secrets set "MongoDb:ConnectionString" "some_random_value" --project spellbound-api/spellbound-api.csproj
```

## Unit Tests

Using xUNit, see tutorials:

- [C# unit testing with xunit](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- [Testing controllers](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-2.1)

## TODO

- Updated CORS policy
- Add authentication
- Add rest of the Spells API from spellbound project
- Add User API
- Add Character API
- Add MongoDB Docker container
- Unit tests