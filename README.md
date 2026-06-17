# ![.NET CI](https://github.com/pavel-ryzhakov/FitFlow/actions/workflows/dotnet-ci.yml/badge.svg)
FitFlow CRM

FitFlow CRM is a backend pet project for managing a fitness club.
The system allows administrators to manage clients, trainers, sections, memberships, training sessions, visits and payments.

## Tech Stack

* C#
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* Docker Compose
* Swagger / OpenAPI
* Clean Architecture style project structure

## Project Structure

```text
FitFlow
├── src
│   ├── FitFlow.Api
│   ├── FitFlow.Application
│   ├── FitFlow.Domain
│   ├── FitFlow.Infrastructure
│   └── FitFlow.Worker
└── tests
```

## Modules

### Clients

Client management:

* create client
* update client
* get client by id
* get clients list
* deactivate client

### Trainers

Trainer management:

* create trainer
* update trainer
* get trainer by id
* get trainers list
* deactivate trainer

### Sections

Fitness club sections:

* boxing
* yoga
* crossfit
* swimming
* fitness

Each section is linked to a trainer.

### Memberships

Membership management:

* create membership
* update membership
* get memberships by client
* cancel membership
* track visit limit

### Training Sessions

Group training schedule:

* create training session
* update training session
* get sessions by section
* get sessions by trainer
* control max participants

### Visits

Client visit registration:

* check active client
* check active membership
* check membership dates
* check visit limit
* register visit
* increase used visits count

### Payments

Payment management:

* create payment
* get payments by client
* get payments by membership
* refund payment

## Getting Started

### Requirements

* .NET SDK
* Docker
* Docker Compose
* Git

### Run PostgreSQL

```bash
docker compose up -d
```

### Apply EF Core migrations

```bash
dotnet ef database update --project src/FitFlow.Infrastructure --startup-project src/FitFlow.Api
```

### Run API

```bash
dotnet run --project src/FitFlow.Api
```

### Swagger

Open Swagger in browser:

```text
https://localhost:<port>/swagger
```

or

```text
http://localhost:<port>/swagger
```

## Database

Default local connection string:

```text
Host=localhost;Port=5432;Database=fitflow_db;Username=postgres;Password=postgres
```

## API Modules

```text
/api/clients
/api/trainers
/api/sections
/api/memberships
/api/training-sessions
/api/visits
/api/payments
```

## Roadmap

Planned improvements:

* FluentValidation
* JWT authentication
* roles: Admin, Manager, Trainer
* Redis caching
* RabbitMQ integration
* background worker
* unit tests
* integration tests
* GitHub Actions CI
* global exception middleware
* Serilog logging

## Purpose

This project is created as a portfolio backend project for C#/.NET developer interviews.
It demonstrates practical backend development with ASP.NET Core, PostgreSQL, EF Core, layered architecture and real business logic.
