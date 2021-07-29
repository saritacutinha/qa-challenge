# Software Developer in Test Technical Challenge

## About the challenge

```
The goal is to add more tests.
```

The application consists of two parts, React SPA and dotnet 3.1 API. The code offers multiple opportunities to introduce tests. Starting from the most valuable

1. Contract tests;
1. Back-end unit tests;
1. Back-end integration tests; 
1. Front-end unit tests; 
1. Front-end component tests; 
1. Front-end end-to-end test;

There are a few tests already to make things smoother. Your task is to add more.

Another area you may consider is building a CI/CD pipeline of your choice, e.g. GitHub Actions, Bitbucket Pipeline, CircleCI, etc.

You may deliver the solution in any form, e.g. GitHub, Bitbucket project or zip archive. If you decide to present a public repository, please consider deleting it after the interview.

There are extra points for finding bugs. 🐛

## Front End Testing Framework : 
1. FrontEnd integration testing : Cypress. (./cypress)
   * cypress/fixtures/todos.json : Test fixture has data to be poulated for Front end intergration tests.
2. E2E tests framework : Cypress. (.cypress/integration/e2e.spec.js)

## PACT Consumer driven contract tests :
1. Test for http verb GET and POST (src/tests/pact/test.spec.ts) using Jest.

## Backend Testing Framework : 
1. XUnit for unit tests.
2. PACT for contract testing with XUnit

Bugs : 
1. No error validations done on failed network requests.
2. Duplicate items dont get deleted and show up on get all items once one of the items are deleted.

Improvements : 
1. Test cases to check user input validations.
2. Test case to check mandatory fields in the DB.
3. F/E unit tests.
4. Negative test cases
5. Test coverage.


## About the application

The application offers a simple Todo List. It consists of several features

1. Add, edit and remove items;
1. Mark items as completed;
1. Filter completed and active;
1. Bulk remove completed items;
1. Deduplicate items;
1. Hide outdated items;

The application requires both back- and front-end running to be fully operational.

---

### Server - provider

Based on [netcoreapp 3.1](https://dotnet.microsoft.com/)

**Prerequisites**

1. [Dotnet Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) installed
1. `Makefile` (if you're on Windows, please consider [this](https://stackoverflow.com/questions/32127524/how-to-install-and-use-make-in-windows) or use `dotnet` commands from Makefile)

**Usage**

1. Navigate to `/qa-ct-backend`
1. Execute `make start`

As a result, you should be able to hit

1. Swagger documentation at https://localhost:5001/swagger/index.html
1. API at https://localhost:5001/, e.g. https://localhost:5001/api/TodoItems

**Run tests**

1. Make a copy of `qa-ct-backend/Todo.ContractTests/.env.sample` and rename it to `.env`
1. Provide `PACT_BROKER_BASE_URL` and `PACT_BROKER_TOKEN` values
   ```
   PACT_BROKER_BASE_URL=https://<your-pactflow-domain>.pactflow.io
   PACT_BROKER_TOKEN=<see https://<your-pactflow-domain>.pactflow.io/settings/api-tokens for a token>
   ```
   You may create a free Pactflow account [here](https://pactflow.io/pricing/).
1. Run all tests with `make test`
1. or run provider contract tests with `make test-pact`

---

### Client - consumer

Based on [Create React App](https://reactjs.org/docs/create-a-new-react-app.html)

**Prerequisites**

1. [Node.js](https://nodejs.org/en/) v14 installed, if you use [nvm](https://github.com/nvm-sh/nvm) you may execute `nvm use` to read the version from `qa-ct-frontend/.nvmrc`

**Usage**

1. Navigate to `/qa-ct-frontend`
1. Execute `npm start`

As a result, your default browser should pop up and present the Todo application at http://localhost:3000/

**Run tests**

1. Make a copy of `/qa-ct-frontend/.env.sample` and rename it to `.env`
1. Provide `PACT_BROKER_BASE_URL` and `PACT_BROKER_TOKEN` values
   ```
   PACT_BROKER_BASE_URL=https://<your-pactflow-domain>.pactflow.io
   PACT_BROKER_TOKEN=<see https://<your-pactflow-domain>.pactflow.io/settings/api-tokens for a token>
   ```
   You may create a free Pactflow account [here](https://pactflow.io/pricing/).
1. Run all tests with `npm test`
1. or run consumer contract tests with `npm run test:pact`




