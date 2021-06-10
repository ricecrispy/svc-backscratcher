# svc-backscratcher
Backscratcher API

## Purpose
A simple Rest API that provides CRUD operations to backscratcher objects.

## Built With
[C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - The C# language

[.Net Core](https://dotnet.microsoft.com/) - free and open-source, managed computer software framework for Windows, Linux, and macOS operating systems

[Npgsql](https://www.npgsql.org/) - open source ADO.NET Data Provider for PostgreSQL

[Dapper FluentMap](https://dapper-tutorial.net/dapper-fluentmap) - a small library which allows you to fluently map properties of your domain classes to the database columns.

[AutoMapper](https://automapper.org/) - a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another. 

## Prerequisites
[Docker](https://www.docker.com/)

[.Net Core SDK and/or Runtime](https://dotnet.microsoft.com/download/dotnet)

Code editor (ex. Visual Studio Code)

## Getting Started:

### Local Development

1. Clone the repository
```
git clone https://github.com/ricecrispy/svc-backscratcher
cd svc-backscratcher
```

2. Open `svc-backscratcher.sln` to start editing the project files. (Please refer to the `Configuration` section for changing values in appsettings.json)


## Configuration

In the `appsettings.json` file, there is a property named `PostgreSqlDataAccessOptions`. The following properties can be modified to connect to the desired database:
- Host: the host name of the database.
- Port: the port of the database. `5432` is the default value.
- Username: the username of the user for accessing the database. `postgres` is the default value.
- Password: the password of the user for accessing the database.
- Database: the name of the database.

## API Operations
Backscratcher model:
- identifier: the ID of the backscratcher.
- name: the name of the backscratcher.
- description: the description of the backscratcher.
- sizes: the available sizes for the backscratcher. Valid sizes are S, M, L, XL.
- price: the price of the backscratcher.

### POST

Route:
```
/backscratchers
```
Body example:
```
{
    "name": "Glamorgirl",
    "description": "Pretty app",
    "price": "$9,343.00",
    "sizes": ["XL"]
}
```

note: identifier field is ignored for POST requests.

Response:
- 201: Created object successfully. The Identifier of the newly created object is returned.
- 400: Invalid body parameter(s).
- 409: The object already exists.

### GET
Routes:
```
/backscratchers
```
note: this request returns all backscratchers.

```
/backscratchers?name=<NAME>&description=<DESCRIPTION>&sizes=<SIZES>&price=<PRICE>
```
note: this request has the following optional query parameters: name, description, sizes, and price.
- name: if this is included, the request returns an array of backscratchers with their names matching this parameter.
- descritpion: if this is included, the request returns an array of backscratchers with their descritpions containing this parameter.
- sizes: if this is included, the request returns an array of backscratchers with sizes matching this parameter. This parameter is a comma seperated list. ex. sizes=S,XL.
- price: if this is included, the request returns an array of backscratchers with prices lower or equal to this parameter. 

Response:
- 200: OK. An array of backscartcher is returned.
- 400: Invalid query parameter(s).

```
/backscratchers/<BACKSCRACTCHER_ID>
```
note: this request returns a backscratcher with matching path parameter value. 

Response:
- 200: OK. A backscartcher is returned.
- 400: Invalid path parameter.
- 404: the object to get is not found.

### PUT

Route:
```
/backscratchers/<BACKSCRACTCHER_ID>
```

Body example:
```
{
    "identifier": "e0e9245c-9032-48e8-972c-cea168ac5042",
    "name": "Glamorgirl-updated",
    "description": "Pretty app updated",
    "price": "$9999.99",
    "sizes": ["S", "XL"]
}
```

note: identifier field is required for PUT requests.

Response:
- 200: Updated object successfully.
- 201: Created object successfully if the object does not exist. The Identifier of the newly created object is returned.
- 400: Invalid path and/or body parameter(s).
- 404: the object to update is not found.

### DELETE

Route:
```
/backscratchers/<BACKSCRACTCHER_ID>
```

Response:
- 200: Deleted object successfully.
- 400: Invalid path parameter.
- 404: the object to delete is not found.