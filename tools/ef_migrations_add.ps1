param (
    [Parameter(Mandatory=$true)]
    [string]$name
)


if (-not $name) {
    Write-Host "Usage: ef_migrations_add.ps1 -name <migration_name>"
    exit 1
}

dotnet ef migrations add $name --project ExampleEFCoreMigrate --context ApplicationDbContext --output-dir Migrations