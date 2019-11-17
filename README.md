## About

This is the web application of Sérgio Bueno portfolio.
This project was implemented in ASP.NET Core 2.2 

*This project requires Docker!*

It was deployed into Heroku server. Check it out at https://sergiobuenoportfolio.herokuapp.com/

---

## Resolve dependencies

1. dotnet restore

---

## Publish application

1. dotnet publish -c Release -o out

---

## First time to deploy into Heroku server

1. heroku login
2. heroku container:login
3. docker build -t <image-name> <path-where-application-was-published-within-dockerfile-inside>
4. docker tag <image-name-created-in-previous-step> registry.heroku.com/<heroku-app-name>/web
5. docker push registry.heroku.com/<heroku-app-name>/web
heroku container:release web --app <heroku-app-name>

## Re-deploy into Heroku server

1. docker build -t <same-image-name> <path-where-application-was-published-within-dockerfile-inside>
2. heroku container:push web -a <heroku-app-name>
3. heroku container:release web -a <heroku-app-name>
