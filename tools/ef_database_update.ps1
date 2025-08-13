param (
    [Parameter(Mandatory=$true)]
    [string]$name
)

dotnet ef database update $name --project ExampleEFCoreMigrate --context ApplicationDbContext