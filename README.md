# MyPortfolio

This is the project application of Sergio Bueno portfolio

### About

This is the web application of the Sérgio Bueno Portfolio.
It was mainly implemented in ASP.NET MVC Core 6

Check it out at http://www.sergiobueno.me/

### Technical requirements

- ASP.NET Core 6
- Docker
    - mcr.microsoft.com/dotnet/sdk:6.0 (image)
    - mcr.microsoft.com/dotnet/aspnet:6.0 (image)
- Google Cloud Platform
- PostgreSQL
- Google APIs
    - Geocoding
    - Maps JavaScript

### Pipelines

This project contains continuous integration (CI) and continuous deployment (CD).
Basically for all branches, after a push to the remote a pipeline is trigged which check for all libraries, build the solution, and run all unit tests.

*On the master branch, there is one more step called 'Deploy Application' where the pipeline automatically deploy the application in Heroku via Heroku CLI and Docker integration*

### Test coverage

This project has only one level of test coverage: Unit tests.
*Tests have been using 'Moq' to mock the database*

### Manually commands

- Resolve solution dependencies:
    1. dotnet restore
- Build solution:
    1. dotnet build
- Run all solution tests:
    1. Inside TestCoverage directory
    2. dotnet test
- Publish portfolio application:
    1. Inside MyPortfolio directory
    2. dotnet publish -c <publish mode e.g. Release> -o <destination folder>
- Create local PostgreSQL (inside Container)
    1. Inside MyPortfolio directory
    2. docker-compose up -d