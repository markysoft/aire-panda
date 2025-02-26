# AIRE Logic Patient Appointment Backend Sample

Technical test for AIRE Logic as described in the [Patient-Appointment-Backend](https://github.com/airelogic/tech-test-portal/tree/main/Patient-Appointment-Backend) repository.

## Running the project

The easiest way to build and run the project and tests is from Visual Studio, or your IDE of choice.

Prior to running the project the database migrations will need to be run, via the Package Manager Console for the 
PandaAPI project:
```
update-database
```

Alternatively use the command line tools. 
[Full documentation](https://learn.microsoft.com/en-us/ef/core/cli) on using the CLI is available, but to summarise:

Install the CLI tools for your operating system, i.e. 
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.406-windows-x64-installer

Install the EF CLI extension:
```
dotnet tool install --global dotnet-ef
```

Navigate to the `PandaAPI` folder in your terminal and run the following command:
```
dotnet ef database update
```

Run the application:
```
dotnet run
```

When starting up the command line will print out the URL, i.e. [http://localhost:5087](http://localhost:5087). 
The swagger documentation and tools for calling the API can be found at 
[http://localhost:5087/swagger/index.html](http://localhost:5087/swagger/index.html).

All tests can be run from the root folder (i.e. one level up from where the migration was run) with the command:
```
dotnet test
```

## Introduction

### What I didn't implement
The spec states that it is not expected for all the functionality to be implemented.
Here's a summary of what I didn't implement to complete the exercise is a timely manner:
- validation of NHS Number
- validation of Postcode
- No localisation
- there were no requirements for listing or searching, so none implemented
- I didn't follow the snake case format as implied in the sample data
- The appointment ID guid is provided by the caller

The validation of the duration and status fields demonstrate the approach I would take, I figured in a 
real project there would be existing implementations at Aire for NHS number validation, so would look 
there first.

Changing the format to snake-case would be easier enough if required, as would changing the appointment ID Guid to 
be a database generated ID.

### Focus of Functionality

Based on the requirements, I focused on the following functionality:

- Database persistence for patients and appointments
- Implementation of business logic for cancelled and missed appointments 
- Correct handling of diacritial characters in patient names
- Ensuring the implementation is extendable to support future requirements
- Unit and integration tests

Handling special characters seemed important as there are legal repercussions to incorrectly
holding personal data. It's straight forward to implement, so a quick win to build trust with
a client.

My goal was to provide a foundational structure to the API that was suitable to build upon, demonstrating
what I believe are best practices in implementation and testing.

## Tech Stack

I selected .NET 8 as the technical stack. Standard Microsoft libraries and frameworks were used
as the client wishes to avoid vendor lock-in and work with smaller frameworks.

For the ORM layer I chose Entity Framework (EF). The requirements say there is a desire to be 
database agnostic. While this ties the project to EF, EF does support a 
[number of database providers](https://learn.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
which should allow for migration to a popular alternative if the need arises.

I selected [SQLite](https://www.sqlite.org/index.html) as the database provider. This is a fully 
featured lightweight SQL database that is persisted to a file making for easier testing and setup.

### Implementation

A RESTful API was implemented using the 
[ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0) framework.

[Controllers](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0)
were used rather than 
[Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview?view=aspnetcore-8.0)
as they provide a standard way to implement RESTful APIs and may be more familiar to developers.

API Endpoints are documented using [Swagger](https://swagger.io/), providing information on the request and
response, and example values to submit.

### API Strucuture

The controllers were kept very thin, primarily returning appropriate responses, and delegating business logic to
other services.

All of the objects passed in to API endpoints are DTOs, separating the back end representation of the data
from that exposed to the client. 

Given the simplicity of the service and the time constraints, I didn't explicity create a service layer.
The DTO is passed directly to the repository, with business logic handled in the Patient and Appointment 
mappers. In hindsight, maybe I should rename the `repositories` to `services`.

Errors in mapping or persisting records throw an application specific `PandaApiException` which are caught by 
error handling middleware and returned as an appropriate 400 response.

This removes the need for error handling in the controllers, keeping them small.

### Testing

Given the time contraints, I was pragmatic about testing. I wanted unit tests to cover the business logic, 
primarily to ensure objects are mapped back and forth correctly, dates are handled as expected, and the validation
logic is verified. These tests can be found in the `PandaAPI.Test.Unit` project.

I added integration tests  at `PandaAPI.Test.Integration` to ensure the records are persisted and retrieved 
correctly from the database.

I also added a test to call the API endpoint directly through the full stack. I've found the 
[WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0)
for integration testing isn't as used as much as I'd hope on new projects, so getting it in early is beneficial.

### Areas to highlight

I spent a fair amount of time considering how I was going to represent the patient and appointment data,
especially the appointment duration.

One particular challenge was handling the `Status` for missed appointments. 
I didn't want the status to need to actively set by the client when an appointment had been missed.

Instead I mapped the duration to an `EndTime`, and made the `missed` status a computed propertly so an `active` 
appointment with an `EndTime` in the past will have a status of `missed`.

Having the appoint duration in this format makes it easier to query for missed appointments, and group them
by clinician or department, which will be useful for future requirements of tracking missed appointments.

### Potential feature creep?

For an update via a `PUT` I decided to ensure the identifier (i.e. NHS Number or Appointment ID) in the body is 
the same as that in the URL. I felt that the resource ID shouldn't change, as it can lead to exploits.

### Technical debt I would address with more time

The status is being held as a string, it would be better to have an enum, and the `Status` normalised into its 
own database table. Perhaps this could be done in future when clinicians and organisations are normalised and
moved into their own tables.

You also might want to check if a record exists before creating a new one, or updating an existing one, rather than 
relying on an underlying exception being thrown. There's pros and cons of each approach.

The default behaviour for the EF model is to cascade patient delete removing the appointment too, this may not 
be desired.

I'd also like to have had the time to improve the Swagger documentation with more details and examples of the 
format, rather than putting it in the 'remarks'.

I'd also like to have added style checking to the project, to ensure a consistent code style.
