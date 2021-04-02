FROM microsoft/dotnet:2.1-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
COPY spellbound-api/spellbound-api.csproj ./
RUN dotnet restore

# copy and build everything else
COPY spellbound-api/* ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/spellbound-api.dll"]