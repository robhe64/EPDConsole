# Chipsoft EPD Assignment: Design choices
*Rob Hellemans, 2025-08-10*

## Architecture
The application is built on a traditional N-tier layered architecture with the following layers:
- **Presentation Layer** (`Chipsoft.Assignments.EPD.Console`): User interface and interaction handling
- **Business Logic Layer** (`Chipsoft.Assignments.EPD.BLL`): Business rules, validation, and orchestration
- **Data Access Layer** (`Chipsoft.Assignments.EPD.DAL`): Data persistence and retrieval
- **Domain layer** (`Chipsoft.Assignments.EPD.Domain`): Core business entities

Dependencies flow in a single direction, from the presentation layer down to the data layer.
Each layer interacts with the one below it through interfaces, following the dependency inversion principle.

A single exception to this dependency flow is the "reset database" option, where the 
presentation layer directly accesses the DbContext. This option is only for testing purposes and wouldn't be present in 
a normal application, so I didn't feel the need to make separate manager and repository methods for this.

### Benefits
This architecture maintains a clear separation of concerns, with each layer responsible for its own tasks. Due to this,
the application can easily swap out its console interface for a different presentation layer (e.g., a web API) without 
affecting the business logic and data access layers. 

### Tradeoffs
The application currently groups layers together in separate projects. If the application grows larger, there is a risk 
of bloated layers with too many responsibilities. In such cases, grouping together features instead of layers might 
become preferable.

## Testing
The application contains a fifth project for tests (`Chipsoft.Assignments.EPD.Tests`).

The layered architecture makes it easy to create unit tests for different components of the application. I decided
to make unit tests for the business layer managers to test validation logic, as well as integration tests for the 
data access layer to test cascading rules. I decided not to test the presentation layer, as creating tests for console
applications is cumbersome. If the presentation layer had been different (e.g., a web API), I would have likely opted for
integration tests on the endpoints. 

## Technologies used
- **.NET 8**: Upgraded from .NET 6 as .NET 8 is currently the latest LTS version and the only one still receiving support
- **FluentValidation**: Library used for creating validation rules
- **xUnit**: Testing framework used for all unit and integration tests
- **Moq**: Mocking library used in unit tests to mock dependencies