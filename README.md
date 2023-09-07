
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
- [ ] Better UI


## Onion Architecture

According to Jeffrey Palermo: "The main premise is that it controls coupling. The fundamental rule is that all code can depend on layers more central, but code cannot depend on layers further out from the core. In other words, all coupling is toward the center. This architecture is unashamedly biased toward object-oriented programming, and it puts objects before all others."

![Onion architecure](https://github.com/hgdiaz/CardManagement/blob/main/img/onion.png?raw=true)

Source: https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/

The main idea with this project is to make a simple implementation of this architecture, without having several projects or other artifacts.


## Running the project
To run this project you need Visual Studio 2022 with dotnet core 6 installed. Also, you'll need SQL Server (2019 or upper)

After cloning the project, the first thing to do is create tthe DB. Go to SSMS and add a new database called **Cards**. After that, run the scripts under the "scripts" folder. First the "**Schema.sql**" script and then de "**Data.sql**"
Now you have 2 users in the DB: "**admin**" with the pass "**Qwe.123**" and "**user**" with the same password.

Now that you have the DB, go to the src folder, open the solution file **Cardmanagement.sln** with VS.
The nuget packages will be restored automatically.
Select the Cardmanagement.API project and press F5. You'll see the Swagger UI to call the endpoints.
Before calling the endpoints, you must login with a valid user.
Just call the /api/Authenticate/login endpoint and fill the credentials. You'll get a JWT in the response. Copy this token and in the Authorize button in the upper right. In the Value textbox, type the word **Bearer** and then paste your token. The click on the authorize button. That´s all....now you can call the endpoints (if the user has the permission to call it).

In order to run both projects at the same time (the API and the Blazor project), under the Solution Explorer right click the solution -> Properties  and select Multiple Startup projects. Set the Action equal to Start for the Cardmanagement.API and Cardmanagement.WebUI projects, like the image below:
![Multiple startup projects](https://github.com/hgdiaz/CardManagement/blob/main/img/Multiple_startup.jpg?raw=true)

## About the functionality

**User Story Description:**

As a cardholder, I want to be able to manage my cards through a web API application, so that I can perform CRUD (Create, Read, Update, Delete) operations on my cards. Each card should have the following information: card number, cardholder name, expiration month, expiration year, and CVC number.

**Acceptance Criteria:**

**1. Create a Card**

-   As a user, I want to be able to create a new card by providing the following details:
    -   Card Number
    -   Cardholder Name
    -   Expiration Month
    -   Expiration Year
    -   CVC Number
-   The API should validate the input data to ensure it meets the following criteria:
    -   Card Number must be a valid credit card number with 15 digits.
    -   Cardholder Name must be a non-empty string.
    -   Expiration Month must be a valid month (1-12).
    -   Expiration Year must be a valid year between 2023 and 2050.
    -   CVC Number must be a valid CVC code (3 digits)
-   Upon successful creation, the API should return the newly created ID.

**2. Read Card Details**

-   As an admin, I want to retrieve the details of a specific card by providing its unique ID. The API should return the card's details, including the card number, cardholder name, expiration month, and expiration year and CVC.

**3. Update Card Details**

-   As an admin, I want to update the details of an existing card by providing its ID along with the fields I want to update. I should be able to update the following fields:
    -   Number
    -    Cardholder Name
    -   Expiration Month
    -   Expiration Year
    -   CVC Number
-   The API should validate the input data for correctness and ensure it meets the criteria mentioned in the "Create a Card" section.


**4. Delete a Card**

-   As an admin, I want to delete a card by providing its ID
-   The API should remove the card from the system

**5. List All Cards**

-   As a user, I want to retrieve a list of all cards.
-   The API should return a  list of card summaries, including the card number and cardholder name.

**6. Security and Authentication**

-   The API should enforce secure authentication mechanisms to ensure that only authorized users can access and manage their cards.
-   Authentication can be implemented using tokens.

**7. Error Handling**

-   The API should provide meaningful error messages and appropriate HTTP status codes for various scenarios, such as validation failures, unauthorized access, or server errors.

**8. Logging**

-   Implement logging to record API usage and errors for monitoring and troubleshooting purposes.


 