# Functionality overview

This simple web-enabled application is designed to provide a monthly overview of 
the account balances for different departments within a company. 
The CFO, can use this tool to quickly assess the financial status of various 
accounts without delving into the complexities of software development. 
There are 5 accounts in the company and they are

- R&D
- Canteen
- CEO's car expenses
- Marketing
- Parking fines"

# How it works

## Backend

This backend project was developed using the Unit Of Work design pattern.
APIs were developed to,

- POST: Authenticate an user
- POST: Refresh the access token
- GET: Get an array of all accounts in the system
- GET: Get an array of transactions made
- GET: Get an array of transactions made between a provided date range
- POST: Update the account balances with a csv file

APIs are authenticated according to user's Role. Only the login api is public. Others will be authenticated with a JWT.
Token will expire in every 5 minutes and the `refresh` api will regenerate a new access token.

## Frontend

User can navigate between Upload Balances, Account Balances and Account Reports pages after getting logged in to the system. Admin users can view all 3 pages. Viewers can only view the Account Balances page.

The access token is attached to the all API requests other than login. If the token expire when calling an api, refresh token api is called from the interceptor and the failed api will retry with the new token.

Aditionally a dark mode theme is developed for better UX.

# Prerequisites

- Visual Studio 2019 updated version 16.8 or later
- .NET SDK 5.0 or Later
- Visual C++ Redistributable
- MySQL Workbench
- Node.js installed

# Getting started

Clone project using this command

`git clone https://github.com/LasithaPrabodha/account-balance.git`

## Backend

- Navigate to ABVServer folder
- Open the project solution using Visual Studio IDE
- Run the project by clicking the Debug button or press F5
- To run unit tests, go to `Tests -> Run All Tests`

## Frontend
- Navigate to ABVClient folder
- Run web application using `npm start` command
- Run tests using `npm run test`
