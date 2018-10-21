# Overview

ASP.NET API for spellbound. Swagger has been included for documentation [Swagger Documentation](https://localhost:5001/swagger)

## Secret storage

Using [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.1&tabs=macos) for setting secrets in dev environment.

``` bash
dotnet user-secrets set "Movies:ServiceApiKey" "12345"
```

## TODO

- Updated CORS policy
- Add authentication
- Add rest of the Spells API from spellbound project
- Add User API
- Add Character API
- Add MongoDB Docker container