# svc-backscratcher
Backscratcher API

## Purpose
A simple Rest API that provides CRUD operations to backscratcher objects.

## Built with
[C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - The C# language

[.Net Core](https://dotnet.microsoft.com/) - free and open-source, managed computer software framework for Windows, Linux, and macOS operating systems

[Npgsql](https://www.npgsql.org/) - open source ADO.NET Data Provider for PostgreSQL

[Dapper FluentMap](https://dapper-tutorial.net/dapper-fluentmap) - a small library which allows you to fluently map properties of your domain classes to the database columns.

[AutoMapper](https://automapper.org/) - a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another. 

## Prerequisites
[Docker](https://www.docker.com/)

[.Net Core SDK and/or Runtime](https://dotnet.microsoft.com/download/dotnet)

## Getting Started:

### Local Development

1. Clone the repository
```
git clone https://github.com/ricecrispy/svc-backscratcher
cd svc-backscratcher
```

2. Open `svc-backscratcher.sln` to edit the project files

3. Start the service in a docker container
```
docker compose up
```