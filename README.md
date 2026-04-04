#  ShivamFinlytics API

**ShivamFinlytics** is a high-performance Financial Analytics API built with **.NET 10**. This project follows Clean Architecture principles to provide a secure, scalable, and resilient backend for managing personal or business finances.

## Tech Stack

* **Framework:** .NET 10 (ASP.NET Core)
* **Database:** Microsoft SQL Server 2025
* **ORM:** Entity Framework Core 10
* **Security:** JWT (JSON Web Tokens) Authentication
* **Resilience:** Built-in Fixed Window Rate Limiting
* **Architecture:** Clean Architecture (API, Application, Infrastructure, Domain)

##  Key Features & Middleware

### Rate Limiting
To prevent API abuse and Ensure High Availability (HA), the system implements a **Fixed Window Rate Limiter**:
* **Policy:** 5 requests per 1-minute window per user.
* **Queueing:** Allows a maximum of 2 requests in the queue before rejection.
* **Response:** Returns `429 Too Many Requests` when limits are exceeded.

### JWT Authentication
Secure communication is handled via JSON Web Tokens. All sensitive endpoints (Transactions, Dashboard, Insights) require a valid `Bearer` token.
* **Validation:** Signature, Issuer, Audience, and Lifetime validation are enforced.

### Activity Logging
Every critical action—such as logins, transaction updates, and profile changes—is tracked via a dedicated `IActivityLogService` for audit trails and security monitoring.

###  Auto-Migrations
The API is configured to automatically apply Entity Framework Core migrations to **SQL Server 2025** on startup, ensuring the database schema is always in sync with the code.

## Project Structure

* **`ShivamFinlytyics.API`**: Controllers,Rate Limiting and Middleware.
* **`ShivamFinlytics.Application`**: Business logic, Interfaces, and Services (Auth, Transactions, Insights).
* **`ShivamFinlytics.Infrastructure`**: Data access, DbContext, and SQL 2025 Migrations.
* **`ShivamFinlytics.Domain`**: Core Entities and Domain Models.

* ## Clean Architecture Pattern
This project is divided into four distinct layers to ensure separation of concerns and testability:
- **Domain**: Contains Enterprise logic (Entities, Enums, Exceptions).
- **Application**: Contains Business logic (DTOs, Mappings, Service Interfaces).
- **Infrastructure**: Handles Data Access (EF Core, SQL Server 2025, Identity).
- **API**: The entry point (Controllers, Rate Limiting, JWT Middleware).

## Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* SQL Server 2025

### Local Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/Shivam2504/ShivamFinlytics.git](https://github.com/Shivam2504/ShivamFinlytics.git)
    cd ShivamFinlytics
    ```

2.  **Configure Environment:**
    Set the following variables in your environment or a `.env` file:
    * `ConnectionStrings__DefaultConnection`: Your SQL 2025 string.
    * `JWT_KEY`: A 32+ character secret key.
    * `JWT_ISSUER`: e.g., `https://localhost:7000`
    * `JWT_AUDIENCE`: e.g., `https://localhost:7000`

3.  **Run the App:**
    ```bash
    dotnet run --project ShivamFinlytics.API
    ```


API Documentation
All routes are prefixed with /api. Most routes require a JWT Bearer Token in the header:
Authorization: Bearer {your_token}

1. Authentication
AuthController
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| POST | /auth/login | Authenticate user and return JWT | None |

2. User Management
UserController
| Method | Endpoint | Description | Role |
| :--- | :--- | :--- | :--- |
| GET | /user/all | List all registered users | Admin |
| GET | /user/{id} | Get specific user details | Admin |
| POST | /user/register | Create a new user account | Admin |
| PUT | /user/toggle-status/{id} | Activate/Deactivate a user | Admin |
| PUT | /user/update-role/{id} | Change user permissions | Admin |
| GET | /user/admin/logs | View all system activity logs | Admin |
| GET | /user/admin/logs/user/{id} | View logs for a specific user | Admin |

3. Financial Dashboard
DashboardController
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| GET | /dashboard/summary | Get overall financial summary | Required |
| GET | /dashboard/category | Get breakdown by category | Admin |

4. Transactions
TransactionsController
| Method | Endpoint | Description | Role |
| :--- | :--- | :--- | :--- |
| GET | /transactions/all | Get all transactions | Admin |
| POST | /transactions/create | Record a new transaction | Admin |
| DELETE | /transactions/{id} | Remove a transaction | Admin |

5. Financial Insights
InsightController
| Method | Endpoint | Description | Role |
| :--- | :--- | :--- | :--- |
| POST | /insight/Create | Generate a new financial insight | Analyst, Admin |
| GET | /insight | Retrieve all insights | Required |
