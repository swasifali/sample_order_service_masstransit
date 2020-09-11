# Sample Order Service using MassTransit Library

## Pre-requisites

- Docker Desktop for Windows

## RabbitMq

- clone the repo
- run "docker-compose up" from the root folder of the project
- Navigate to http://localhost:15672 to make sure that RabbitMq is running. Use default credentials to login (guest/guest)

## Try the sample

- Make sure to set multiple startup projects (OrderSample.Api and OrderSample.Console)
- Start debugging
- This should start the order service in a console app and the api in browser. You will see the swagger page.
- try out the POST endpoint to create an order and POST /cancel to cancel an order
- the app has no state / persistence


## Solution Structure

### OrderSample.Api
This is the Asp.Net Core API that implements the Order Service. It exposes two endpoints, one for submitting order and another for cancelling it.
The submit order endpoint showcases the request / response communication pattern.
The cancel order endpoint showcases the Command communication pattern.

### OrderSample.Console
This is the console application that hosts the consumers. It's based on .NET Core Generic host and can be run as a console app, service or docker container.
This is just the host and the logic for order service is in the OrderSample.OrderServices project.

### OrderSample.Contracts
This is the shared library that specified the message contracts. MassTransit requires class names (including namespaces) to be same between producers / consumers.

### OrderSample.OrderServices
It covers the main logic for consuming various messages. For simplicity, all consumers are implemented within the same library project but it can be decoupled into
multiple libraries and / or hosted in seprate console apps as well.


