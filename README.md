## About

This is the web application of the Sérgio Bueno Portfolio.
It was mainly implemented in ASP.NET MVC Core 2.2

Check it out at http://www.sergiobueno.me/

---

## Technical requirements

- ASP.NET Core 2.2
- Docker
    - microsoft/dotnet:2.2-sdk (image)
    - microsoft/dotnet:2.2-aspnetcore-runtime (image)
- Heroku
- PostgreSQL
- Google APIs
    - Geocoding
    - Maps JavaScript

---

## Pipelines

This project contains continuous integration (CI) and continuous deployment (CD).
Basically for all branches, after a push to the remote a pipeline is trigged which check for all libraries, build the solution, and run all unit tests.

*On the master branch, there is one more step called 'Deploy Application' where the pipeline automatically deploy the application in Heroku via Heroku CLI and Docker integration*

---

## Test coverage

This project has only one level of test coverage: Unit tests.
*Tests have been using 'Moq' to mock the database*

---

## Manually commands

- Resolve solution dependencies:
    1. dotnet restore
- Build solution:
    1. dotnet build
- Run all solution tests:
    1. dotnet test
- Publish application:
    1. dotnet publish -c <publish mode e.g. Release> -o <destination folder>
- Deploy into Heroku server (via Docker): *This requires Heroku CLI installation*
    1. heroku login
    2. heroku container:login
    3. docker build -t <image-name> <path-where-application-was-published-within-dockerfile-inside>
    4. docker tag <image-name-created-in-previous-step> registry.heroku.com/<heroku-app-name>/web
    5. docker push registry.heroku.com/<heroku-app-name>/web
    6. heroku container:release web --app <heroku-app-name>
- Re-deploy into Heroku server (via Docker): *This requires Heroku CLI installation*
    1. docker build -t <same-image-name> <path-where-application-was-published-within-dockerfile-inside>
    2. heroku container:push web -a <heroku-app-name>
    3. heroku container:release web -a <heroku-app-name>