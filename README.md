# ddb-backend
Backend coding exercise for D&amp;D Beyond

## Running in Docker
```
docker-compose up
```

Navigate to http://localhost:5000/swagger to see the auto-generated Swagger Doc, which lists the API endpoints along with example requests.

## Running in VS Code for debugging
From debugger pane in VS Code, select the ".NET Core Launch (web)" option, and click the green button.

Similarly, navigate to http://localhost:5000/swagger to see the auto-generated Swagger Doc, which lists the API endpoints along with example requests.

## Running the unit tests
```
cd Ddb.Tests
dotnet test
```

## Structure of the code
The code is structured based on [Onion Architecture](https://www.codeguru.com/csharp/csharp/cs_misc/designtechniques/understanding-onion-architecture.html) and makes use of [Domain Driven Design](https://martinfowler.com/tags/domain%20driven%20design.html) principles. Some inspiration is taken from [CQRS](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns) in that the _write model_ (the commands) are unique from the _read model_ (the views).

* Everything specifically related to Dungeons &amp; Dragons is in the **Domain Layer** in the `Ddb.Domain` project.
* Technical concerns such as data persistance are in the **Infrastructure Layer** in the `Ddb.Infrastructure` project.
* API controllers are in the `Ddb.Api` project. This is thought of as specialized infrastructure, and is an entrypoint into the application.
* The "features of the software" are in the **Application Layer** in the `Ddb.Application` project. Important things such as retrieving data and emitting events are orchestrated here, but notably is not concerned with the rules of Dungeons &amp; Dragons, like the Domain Layer is. The API is one entry point to the Application Layer, but in the future there might be other entry points, such as asyncronous messaging (AMQP or SQS) or websockets, for example.

## Things to do to make the app ready for production
* Add authentication and authorization.
* Use persistent storage, such as a database, for storing the Player Characters.
* Add a real implemenation for the message bus.
* Configure infrastructure-as-code using Terraform, or similar tool. Add other infrastructure as needed, such as an API gateway with SSL termination, load balancers, etc.
* Build the CI/CD pipeline.
* Increase unit test coverage.
* Consider adding API level integration tests.
* Consider improving logging using a logging framework like Serilog, and connect to a log aggregation service.
* Consider adding analytics, such as NewRelic.
