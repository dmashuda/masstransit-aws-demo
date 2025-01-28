# MassTransit AWS Demo

This repository demonstrates the use of MassTransit with AWS SQS Transport+localstack with Aspire. 
The project is built using C# and showcases how to integrate Aspire, MassTransit, and AWS components for building distributed applications.

## Features

- Runs 100% local (no aws account required)
- Topography Creation Seperated from Api/Backend Worker Applications
- Aspire app host
  - Distributed tracing between Api/Backend Worker Applications

## Prerequisites

- .NET 9.0 SDK or later [official .NET website](https://dotnet.microsoft.com/download).
- Docker or Podman installed

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/dmashuda/masstransit-aws-demo.git
cd masstransit-aws-demo
```

### Install Dependencies

Ensure you have the .NET SDK installed. You can download it from the 

Running the application also requires docker or podman because 


### Running the Application

To launch the application, use the following command:

```sh
dotnet run --project AppHost 
```

To send a request to the api run [Api/Api.http](Api/Api.http)


## Project Structure
- `Api/` Api web application 
- `AppHost/` Aspire Apphost
- `Backend/` Background worker application that consumes messages 
- `Components/` Class Library For Consumers
- `Contracts/` Class Library for Message Contracts
- `ServiceDefaults/` Aspire ServiceDefaults
- `Setup/` Application that creates Bus Topography (the sqs/sns resources)

## Contributing

This is a demo app, so improvements are welcome via PR

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
