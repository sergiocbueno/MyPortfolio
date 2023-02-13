# MyPortfolio

This is the project application of Sergio Bueno portfolio

### Build status

![Build Status](https://github.com/sergiocbueno/MyPortfolio/actions/workflows/dotnet.yml/badge.svg?branch=master)

### About

This is the web application of the Sergio Bueno Portfolio.
It was mainly implemented in ASP.NET MVC Core 6

Check it out at https://sergiobueno.me

### Technical requirements

- ASP.NET MVC Core 6
- Docker
    - mcr.microsoft.com/dotnet/sdk:6.0 (image)
    - mcr.microsoft.com/dotnet/aspnet:6.0 (image)
- Google Cloud Platform
- PostgreSQL
- Entity Framework (including Migrations for versioning)
- Google APIs
    - Geocoding
    - Maps JavaScript
- Abstract API (Geolocation)

### Migrate Database

This project has been using Entity Framework as an object-database mapper for PostgreSQL. To create a change in database (data or structure) simply follow the commands below:

1. Inside MyPortfolio directory
2. dotnet ef migrations add <MigrationName> -o Database/Migrations

The new migration will be automatically executed by the application in the next execution.

### Pipelines

This project contains continuous integration (CI - Github) and continuous deployment (CD - Google Cloud Platform).
Basically for all branches, after a pull request is created to master or a push directly into master, a pipeline is trigged which checks all packages, build the solution, and run all unit tests.

*On the master branch, there is one more step called 'release' where the pipeline automatically creates a tag and a release inside Github, which also automatically triggers an application deployment inside Google Cloud Platform environment using Docker integration*

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
    2. dotnet publish -c _[publish mode e.g. Release]_ -o _[destination folder]_
- Create local PostgreSQL (inside Container)
    1. Inside MyPortfolio directory
    2. docker-compose up -d