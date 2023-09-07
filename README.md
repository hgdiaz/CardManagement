
# Card Management

A small project to show how to create a Web API in dotnet core 6, implementing the onion architecture.
With this sample you can see how to use CQRS (with Mediatr), Fluent Validation, Dapper, Automapper, Identity with JWT, Swagger, Logging (with Serilog) and other usefull stuff.
Also, there's a simple UI in Blazor Server which connects to the API.

## Features

- [x] CQRS with Mediatr
- [x] Fluent Validation
- [x] Automapper
- [x] Dapper
- [x] Identity
- [x] JWT
- [x] Swagger
- [x] Logging with Serilog
- [x] Blazor Server
- [x] Custom Authentication State Provider
## Pending improvements

- [ ] Use scopes for authorization
- [ ] Better test coverage


## Onion Architecture

According to Jeffrey Palermo: "The main premise is that it controls coupling. The fundamental rule is that all code can depend on layers more central, but code cannot depend on layers further out from the core. In other words, all coupling is toward the center. This architecture is unashamedly biased toward object-oriented programming, and it puts objects before all others."
![Onion architecure](https://github.com/hgdiaz/CardManagement/blob/main/img/onion.png?raw=true)

Source: https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/

The main idea with this project is to make a simple implementation of this architecture, without having several projects or other artifacts.


## Running the project
To run this project you need Visual Studio 2022 with dotnet core 6 installed. Also, you'll need SQL Server (2019 or upper)

After cloning the project, the first thing to do is create tthe DB. Go to SSMS and add a new database called Cards. After that, run the scripts under the "scripts" folder. First the "Schema.sql" script and then de "Data.sql"
Now you have 2 users in the DB: "**admin**" with the pass "**Qwe.123**" and "**user**" with the same password.

Now that you have the DB, go to the src folder, open the solution file **Cardmanagement.sln** with VS.
The nuget packages will be restored automatically.
Select the Cardmanagement.API project and press F5. You'll see the Swagger UI to call the endpoints.
Before calling the endpoints, you must login with a valid user.
Just call the /api/Authenticate/login endpoint and fill the credentials. You'll get a JWT in the response. Copy this token and in the Authorize button in the upper right. In the Value textbox, type the word **Bearer** and then paste your token. The click on the authorize button. That´s all....now you can call the endpoints (if the user has the permission to call it).

## About the functionality
The requirements are:

 1. The card management module with API endpoints to make CRUD operations



Extra requirements:

 