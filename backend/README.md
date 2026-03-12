# Backend Architecture & Architecture Guide
Welcome to the Clarity Backend! This RESTful API is built using **.NET 8 (C#)** and connects to a **MongoDB** NoSQL database. 

This document explains the folder structure, core concepts, and how the data flows from a user request down to the database.

## Folder Structure Overview

```text
backend/
├── Controllers/       # API Endpoints (The Entry Points)
├── Models/            # Data Structures & Database Schemas
├── Services/          # Business Logic & Database Operations
├── Program.cs         # Application Configuration & Bootstrapping
└── appsettings.json   # Environment Variables & Secrets
```

---

## 1. `Controllers/` (The Front Door)
**Purpose:** Controllers listen for incoming HTTP requests (GET, POST, PUT, DELETE) from the Angular frontend, validate the request, and route it to the appropriate Service.

*   **`AuthController.cs`**: Handles user registration and login. It receives credentials, checks with the AuthService, and if valid, returns a JSON Web Token (JWT) that the frontend uses for future requests.
*   **`HabitsController.cs`**: Manages all CRUD (Create, Read, Update, Delete) operations for a user's habits. It extracts the `UserId` from the JWT token to ensure users can only modify their own data.
*   **`TasksController.cs`**: Handles the Kanban board logic. It processes the creation of Monthly, Weekly, and Daily tasks, and updates the `CompletedAt` timestamp when a task is finished.
*   **`PomodoroController.cs`**: Logs focused study time and manages the history of timer sessions.
*   **`AnalyticsController.cs`**: Aggregates data from Habits, Tasks, and Pomodoro sessions to generate the 30-day heatmap arrays and user statistics.

---

## 2. `Models/` (The Blueprints)
**Purpose:** Models define the shape of our data. They dictate exactly what fields exist in our MongoDB collections and what data we expect from frontend requests.

*   **`User.cs`**, **`Habit.cs`**, **`TaskItem.cs`**, **`PomodoroSession.cs`**: These classes map directly to MongoDB collections. They use attributes like `[BsonId]` to tell MongoDB which field is the primary key, and `[BsonRepresentation(BsonType.ObjectId)]` to handle NoSQL ID formatting.
*   **`AuthRequests.cs` / `TaskRequests.cs`**: These are Data Transfer Objects (DTOs). Instead of exposing our exact database schema to the frontend, DTOs define the strict payload we expect for specific actions (e.g., `UpdateTaskRequest` allows updating a title without accidentally overriding the `CreatedAt` date).

---

## 3. `Services/` (The Brains)
**Purpose:** Services execute the actual business logic and perform the database queries. Controllers shouldn't talk directly to the database; they ask Services to do it.

*   **Why use Services?** It separates concerns. If we ever switch from MongoDB to PostgreSQL, we only have to rewrite the Services; the Controllers remain untouched. This is known as the **Repository Pattern**.
*   **`AuthService.cs`**: Secures passwords using `BCrypt` hashing before saving to the database, and generates the cryptographic JWT strings for authentication.
*   **`HabitService.cs` / `TaskService.cs`**: Inject the `MongoClient` configuration and execute MongoDB LINQ queries like `.InsertOneAsync()` or `.ReplaceOneAsync()` to mutate data.

---

## 4. `Program.cs` (The Engine Room)
This file runs the moment the server starts. It is responsible for:
1.  **Dependency Injection (DI):** Registering our Services (e.g., `builder.Services.AddSingleton<ITaskService, TaskService>()`) so Controllers can automatically request them.
2.  **Configuring MongoDB:** Binding the URI from `appsettings.json` to our `MongoDBSettings` class.
3.  **Authentication Middleware:** Setting up the JWT Bearer token validation rules so the `[Authorize]` tags on our Controllers actually block unauthorized requests.
4.  **CORS:** Allowing the Angular frontend (`localhost:4200`) to communicate with the .NET backend (`localhost:5239`) without the browser blocking it for security reasons.

## 5. `appsettings.json`
Stores the configuration variables like the MongoDB connection string and the JWT secret key. In production (like within our Docker container), these values can be overridden by environment variables for security.
