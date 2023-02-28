## Getting Started

### Database

Setting database go to `appsettings.Development.json` change default connection to your database connection

```json
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Database=yourdb;Username=yourusername;Password=yourpassword"
}
```

Run the following command to migrate Database

```shell
dotnet ef database update
```

If the Migrations folder is missing you need to create migration file and run code `database update`

```shell
dotnet migrations add InitialCreate
```

### Run

```shell
dotnet run
```