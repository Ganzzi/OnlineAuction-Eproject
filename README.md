
#1 Instalation
1. Install packages (skip if use visual studio code): 
```
    dotnet restore ./Infrastructure 
    dotnet restore ./DomainLayer 
    dotnet restore ./Application
    dotnet restore ./AuctionOnline
```
2. Update migration:
(optional)dotnet ef migrations Add NewMigration --project ./Infrastructure
```
    dotnet ef database Update --project ./Infrastructure
```
3. Run api server:
```
    dotnet run --project ./AuctionOnline
```

4. Run client: 
```
    cd ClientApp
    npm i
    npm run dev
```