# IdealSolutionsAssessment

> A .NET 9 REST API to manage users and tasks with role-based authorization.  
> Users can update their own profiles and view their tasks. Admins can manage all users and tasks.

---

## 👋 Notes from the Developer

1. I used ChatGPT while working on this — if that's not allowed, I may be disqualified 😅
2. I have very little experience with automated testing, but I'm eager to learn!
3. I added basic role validation (e.g., normal users can't delete others).
4. I kept things simple for a demo: no strongly typed IDs, enums for status, exception filters, custom attributes, etc.  
   But all the requirements are met.

---

## 🧰 Tech Stack

- .NET 9
- Entity Framework Core (In-Memory Database)
- JWT Bearer Authentication
- Swagger
- xUnit & Moq for unit testing
- Serilog

---

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- IDE of your choice (e.g., Visual Studio, VS Code, Rider)

---

## 🔧 Launch Configuration

Check the file:  
`IdealSolutionsAssessment/Properties/launchSettings.json`

---

## ▶️ Running the Project

bash:
dotnet run --project IdealSolutionsAssessment

Swagger UI will be available at:

https://localhost:{port}/swagger

---

## 🧪 Running the Tests   --Tests are written using xUnit and Moq

bash:
dotnet test

---

### Seeded Users (In-Memory)

| Role   | Login  | Password | Name   |
|--------|--------|----------|--------|
| Admin  | admin  | 123456   | admin  |
| User   | user   | 123456   | user   |

### Seeded Tasks (In-Memory)

| Title         | Assigned To | Description                                 | Status  |
|---------------|-------------|---------------------------------------------|---------|
| Admin Task 1  | admin       | This is a task for admin.                   | Pending |
| User Task 1   | user        | This is a task accessible by admin and user.| Pending |
| User Task 2   | user        | This is a task accessible by admin and user.| Pending |



## 🔐 Authentication (JWT)

This API uses JWT Bearer Authentication. You must authenticate to access any endpoint.

Bearer <auth token from api/Login>

Example:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdW...

---

##  API Endpoints

Both = Admin + User

### Authentication

| Method | Endpoint             | Role  | Description             |
|--------|----------------------|-------|-------------------------|
| GET    | /api/login           | Any   | Get auth token          |

### Users

| Method | Endpoint             | Role  | Description             |
|--------|----------------------|-------|-------------------------|
| GET    | /api/user            | Admin | Get all users           |
| GET    | /api/user/me         | Both  | Get current user        |
| GET    | /api/user/{id}       | Admin | Get user by ID          |
| POST   | /api/user            | Admin | Create a new user       |
| PUT    | /api/user/me         | Both  | Update own profile      |
| PUT    | /api/user/{id}       | Admin | Update any user's info  |
| DELETE | /api/user/{id}       | Admin | Delete user             |

### Tasks

| Method | Endpoint             | Role  | Description             |
|--------|----------------------|-------|-------------------------|
| GET    | /api/task            | Admin | Get all tasks           |
| GET    | /api/task/{id}       | Both  | Get task by ID          |
| POST   | /api/task            | Admin | Create new task         |
| PUT    | /api/task/{id}       | Both  | Update any task         |
| DELETE | /api/task/{id}       | Admin | Delete task             |

---

## Serilog
Simple logger will log every request you hit in the console 