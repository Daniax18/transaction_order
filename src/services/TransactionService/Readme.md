dotnet ef migrations add InitialCreate --output-dir Infrastructure/Adapter/Outbound/Persistence/Migrations

dotnet ef database update
run en local avec "LocalDb" si transaction-service n'est pas encore actif dans le docker, sinon, DockerDb