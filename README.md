
# Bidhub - Online Auction Website

## Description:
Bidhub is an online auction website built using .NET Core, Next.js, TypeScript, Tailwind CSS, and other technologies. It follows the Onion Architecture pattern and utilizes features such as JWT authentication, Redis caching, SQL Server database, SignalR for real-time updates, and Cloudinary for image management. Bidhub provides a platform for users to buy and sell items through auctions, with features tailored for both admin and regular users.

## Features:
- Admin Role:
    + View list of users and manage user accounts (lock/unlock).
    + Manage categories: view list, view category details, update category, and add/remove items to/from categories.
    + View list of items, item details, and manage items (add/remove categories).

- User Role:
    + Authentication: login, signup, reset password.
    + Security: JWT and refresh tokens for secure authentication.
    + Profile management: view and update profile details.
    + Auctions: view categories with top 4 bidded items, view item list with search, sort, and filter options.
    + Selling: list items for auction, update item details.
    + Bidding: place bids on items, view auction history.
    + Notifications: receive notifications for auction events.

## Tech Stack:

- .NET Core (C#)
- Onion Architecture
- Specification Pattern
- JWT Authentication
- Redis Caching
- SignalR
- SQL Server
- Next.js
- TypeScript
- Tailwind CSS
- Cloudinary

## Installation

### Prerequisitive
Ensure you have the following installed in your development environment:

* docker
* nodejs
* dotnet ef
* Access to [cloudinary](https://cloudinary.com) website and create your new environment and then store your cloudname, api key and api secret to use latter in console -> dashboard.
* Set-up an email that enable Two-Factor Authentication (2FA) and generated an App-Password.

### Install Locally
Follow these steps to set up and run Bidhub locally on your machine.

1. Clone from git:
```
    git clone https://github.com/Ganzzi/OnlineAuction-Eproject.git \
    && cd OnlineAuction-Eproject
```

2. Set up environment variables: 
    - Rename appsettings.sample.json in AutionOnline directory to appsettings.json and fill in these fields: 
        + CloudName
        + ApiKey
        + ApiSecret
        + Mail
        + Password
    - Rename .env.local.sample in ClientApp directory to .env.local

3. Run Redis & SQL Server docker images (and set up database):
```
    sudo docker run --rm \
    --name some-redis \
    -p 6379:6379 \
    -d redis:latest redis-server \
    --save 60 1 \
    --loglevel warning \
    && sudo docker run --rm \
    -e "ACCEPT_EULA=Y" \
    -e "MSSQL_SA_PASSWORD=StrongPassword123@" \
    -e "MSSQL_PID=Evaluation" \
    -p 1433:1433 \
    --name localhost \
    -d \
    mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04 \
    && sleep 10 \
    && docker exec -it localhost /opt/mssql-tools/bin/sqlcmd \
    -S localhost \
    -U SA \
    -P 'StrongPassword123@' \
    -Q "CREATE DATABASE AuctionOnline;" \
    && sleep 10 \
    && dotnet ef database Update --project ./Infrastructure
```

4. Run server:
```
    dotnet restore ./Infrastructure  \
    && dotnet restore ./DomainLayer  \
    && dotnet restore ./Application \
    && dotnet restore ./AuctionOnline \
    && dotnet run --project ./AuctionOnline 
```

5. Run client: Open a new terminal at OnlineAuction-Eproject directory and run the following command:
```
    cd ClientApp \
    && npm i \
    && npm run dev 
```