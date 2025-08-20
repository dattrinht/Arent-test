# Health Management Application

The primary goal is to design and develop a C#.Net backend for a health management web application. The application has three main sections: a **Top page** for daily tracking, a **My Record page** for historical data review, and a **Column page** for health-related articles.

## 📋 Requirements Analysis

<img width="1406" height="493" alt="image" src="https://github.com/user-attachments/assets/dc3e9dbd-8cd5-4d18-a8b6-19c889fd0863" />

_System Context Diagram_

### **Functional Requirements**

**1. Top Page (Post-login dashboard)**
- **Date/Achievement Rate Display**
  - **Requirement**: Show current date with percentage (e.g., "5/21 75%").
  - **Assumption**: Achievement rate = (Completed daily excercies / Total daily excercies) × 100.
  - **Solution**: Implement a daily exercise tracking system where users can set targets and mark them as complete. 
- **Weight/Body fat percentage graph**
  - **Requirement**: Display a visual trend of metrics over time
  - **Assumption**: A line chart showing the last 12 months of data.
  - **Solution**: Store daily measurements, calculate averages, and return chart-ready data points.
- **Input transition button**
  - **Requirement**: Quickly filter the meal history data.
  - **Assumption**: Categorize meal history by meal type, such as Breakfast, Lunch, and Dinner.
  - **Solution**: Create a "Meal" entity that includes type information.
- **Meal history**
  - **Requirement**: Display a grid of meals.
  - **Assumption**: The grid on the top page shows meals ordered by date and includes a "load more" feature.
  - **Solution**: The backend will provide an API endpoint that returns a list of meal records with filtering and pagination functionality.

**2. My Record Page (Personal records)**
- **Navigation buttons to recording screens**
  - **Requirement**: Provide navigation to different record types: body records, excercies, and diaries.
  - **Assumption**: Clicking these buttons will navigate to entirely new pages.
  - **Solution**: The backend needs to provide separate API endpoints for fetching exercise records and diary entries. The front end will call the appropriate endpoint when a user clicks a button. 
- **Weight/Body fat percentage graph**
  - **Requirement**: A graph showing weight and body fat percentage trends.
  - **Assumption**: This is the same component as on the Top Page but may offer more detailed filtering options.
  - **Solution**:  A single, flexible API endpoint for body records that accepts date range parameters can serve both this page and the Top Page.
- **Exercise records**
  - **Requirement**: Display a log of the user's past physical activities.
  - **Assumption**: The data should be presented as a scrollable list, with each entry showing details like the exercise name, duration, and calories burned.
  - **Solution**:  The backend will have an `Exercises` table. The API will expose an endpoint to retrieve a paginated list of these records for the logged-in user.
- **Diary entries**
  - **Requirement**: Display a list of the user's personal diary entries.
  - **Assumption**: The view will show a list of entries, likely with a title and a preview of the content.
  - **Solution**:  The backend will have a `Diaries` table. The API will expose an endpoint to retrieve a paginated list of diary entries for the logged-in user.

**3. Column Page (Health articles - public access)**
- **Health-related content viewable without login**
  - **Requirement**: This page and all its content must be accessible to users who are not logged in.
  - **Assumption**: The content is the same for both logged-in and guest users.
  - **Solution**:  The API endpoint for fetching these articles will be public and will not require an authentication token.
- **Recommended categories/filters**
  - **Requirement**: Display filter buttons for different article categories like "Diet," "Beauty," and "Health".
  - **Assumption**: Users can click these buttons to filter the grid of articles shown on the page.
  - **Solution**:  The backend API for fetching articles should accept a query parameter for filtering by category/tag. The database will need a many-to-many relationship between columns and tags.

### **Non-functional Requirements**

**1. Authentication & Profile Management**
- **User Registration & Login**
  - **Assumption**: Authentication uses email and password only. Passwords are stored securely with hashing.
  - **Solution**:  Implement a registration API that creates a user account with hashed credentials. Use JWT-based authentication for login sessions.
- **Profile Management**
  - **Assumption**: Profile information is distinct from authentication credentials and can evolve independently.
  - **Solution**:  Create a `Profile` entity that is linked one-to-one with the `User` entity. The `Profile` stores non-authentication attributes.

## **🏗 System Architecture**

### **High-level architecture**

<img width="1129" height="834" alt="image" src="https://github.com/user-attachments/assets/e4ae087d-d3b6-4ab2-93fb-620ae12596c7" />

_High-level architecture diagram_

**1. Architecture overview**

The system consists of three primary layers:
- **Presentation Layer**: Web client interface for health data interaction
- **Application Layer**: ASP.NET Core API services with specialized health business logic
- **Data Layer**: PostgreSQL database with Redis caching.

#### **Component Responsibilities**

| Component | Primary Responsibility | Health-Specific Function |
|-----------|------------------------|--------------------------|
| **Web Client** | User interface and experience | Health data visualization, goal tracking UI |
| **ASP.NET Core API** | Business logic and data orchestration | Achievement calculations, health analytics |
| **Auth Service** | Security and user management | Secure health data access control |
| **PostgreSQL Database** | Persistent data storage | Health records, meal tracking data |
| **Redis Cache** | Performance optimization | Real-time achievement rates, monthly analytics |
| **Object Storage** | Media content management | Meal photos, article media |

**2. Integration flow**

##### Authentication Flow
- The **Web Client** sends a **Login Request** to the **Auth Server**.
- The **Auth Server** authenticates the user and may use an **External Entity Provider** (e.g., Google, Facebook).
- Upon successful login, a **JWT access token** is issued.
- The client stores the token and includes it in the `Authorization` header for subsequent API requests.

##### API Request Flow
- The **Web Client** makes HTTP requests to the **ASP.NET Web API**, including the JWT token.
- The API validates the token locally using keys from the **JWKS endpoint** exposed by the Auth Server.
- After validation, the API:
  - Performs **Read/Write** operations on the **PostgreSQL** database.
  - Retrieves or updates frequently-accessed data from **Redis** (Distributed Cache) for improved performance.

##### Media Handling Flow (_Not implemented yet_)
- The **Web API** provides the client with **pre-signed upload URLs** for **Object Storage**.
- The client uploads media directly to **Object Storage**.
- The **CDN** fetches and caches this content from Object Storage and delivers it to the client.

**3. Technical choices**

| Layer            | Technology                     | Justification                                                                 |
|------------------|--------------------------------|-------------------------------------------------------------------------------|
| **Backend**      | ASP.NET Core Web API           | Cross-platform, fast, supports REST, async/await, DI, and modular design.     |
| **Auth**         | OAuth 2.0 / OpenID Connect + JWT | Stateless and scalable authentication; supports federation.                  |
| **Database**     | PostgreSQL                     | Reliable, ACID-compliant RDBMS with strong support for JSONB and indexing.   |
| **Cache**        | Redis                          | Fast in-memory store for caching and performance-critical data.              |
| **Storage**      | Object Storage (e.g., S3/Azure Blob) | Durable and scalable file storage for user-uploaded content.             |

### **Application architecture**

**1. Architecture overview**

<img width="1082" height="472" alt="image" src="https://github.com/user-attachments/assets/46f789b5-769b-4305-a566-7fe6d6562b29" />

_Application architecture diagram_

This system uses a clean, layered architecture with clear separation of concerns:

- **API Layer (ASP.NET Core Web API)** — HTTP endpoints, model binding, validation, Swagger.
- **Application Layer** — Use cases (services), specifications.
- **Domain Layer** — Entities, enums, invariants (no external dependencies).
- **Infrastructure Layer** — EF Core (PostgreSQL), repositories, entity configurations, Redis cache, external gateways.

## **📦 Database Design**

- **`users`**: Stores core user credentials and account metadata.
- **`profiles`**: Holds personal information linked to a user, such as name, birthdate, and gender.
- **`meals`**: Tracks logged meals with metadata like type, name, and completion time.
- **`exercises`**: Records physical activities, including duration, calories burned, and status.
- **`body_records`**: Logs body metrics such as weight and body fat over time.
- **`diaries`**: Captures personal journal entries with title, content, and preview.

<img width="836" height="993" alt="image" src="https://github.com/user-attachments/assets/fa6d923d-6a26-4b43-a441-2813e4d699d7" />

_Database design for Healh Tracking module_

### **Health Tracking module**

### **Column module**
- **`columns`**: Stores content such as articles or blog posts.
- **`column_taxonomies`**: Defines classification tags like categories or topics for organizing content.
- **`column_taxonomy_associations`**: Links columns with taxonomies to enable flexible content tagging (many-to-many).
  
<img width="815" height="827" alt="image" src="https://github.com/user-attachments/assets/830b9805-a905-4db0-bf32-fed2bfbe8011" />

_Database design for Column module_

## **📚 API Design**

### Authentication

#### `POST /identities/login`

Authenticate a user using email and password.

##### Request Body:

```json
{
  "email": "email@healthapp.com",
  "password": "password"
}
```

##### Response:

```json
{
  "userId": 1,
  "email": "email@healthapp.com",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### `GET /profiles/fetch`

Fetch paginated list of profiles associated with the specified user.

##### Request Parameters:

| Parameter | Type   | Required | Description                             |
|-----------|--------|----------|-----------------------------------------|
| userId    | long   | Yes      | The ID of the user whose profiles to fetch |
| page      | int    | No       | Page number for pagination (default: 1)    |
| pageSize  | int    | No       | Number of profiles per page (default: 20) |

##### Response:

```json
{
  "items": [
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Top page

#### `GET /exercises/achievement`

Returns the number of exercises scheduled and completed by the profile for a specific day.

##### Request Parameters:

| Parameter | Type   | Required | Description                         |
|-----------|--------|----------|-------------------------------------|
| profileId | long   | Yes      | The ID of the user's profile        |
| day       | string | Yes      | The specific day in yyyy-MM-dd format |

##### Response:

```json
{
  "totalCount": 0,
  "completedCount": 0
}
```

#### `GET /bodyrecords/monthly-averages`

Returns the average body metrics (weight and body fat) per month for the given date range.

##### Request Parameters:

| Parameter  | Type   | Required | Description                              |
|------------|--------|----------|------------------------------------------|
| profileId  | long   | Yes      | The ID of the user's profile             |
| fromMonth  | string | Yes      | Start month in yyyy-MM format            |
| toMonth    | string | Yes      | End month in yyyy-MM format              |

##### Response:

```json
[
  {
    "year": 2025,
    "month": 8,
    "averageWeight": 65.3,
    "averageBodyFat": 15.8
  }
]
```

#### `GET /meals/fetch`

Returns a paginated list of meals based on the profile and meal type.

##### Request Parameters:

| Parameter | Type   | Required | Description                                |
|-----------|--------|----------|--------------------------------------------|
| profileId | long   | Yes      | The ID of the user's profile               |
| type      | short  | No       | The type of meal (e.g., breakfast, lunch)  |
| page      | int    | No       | Page number for pagination (default: 1)    |
| pageSize  | int    | No       | Number of items per page (default: 20)     |

##### Response:

```json
{
  "items": [
    {
      "id": 1,
      "profileId": 1,
      "name": "Grilled Chicken",
      "type": 1,
      "image": "https://cdn.domain.com/images/meal1.jpg",
      "doneAt": "2025-08-20T08:02:15.573Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### My Record page

#### `GET /bodyrecords/monthly-averages`

See: API Design > Top page -> `GET /bodyrecords/monthly-averages`

#### `GET /exercises/fetch`

Fetch a paginated list of exercise records for a specific profile. Optionally filter by a specific date.

##### Request Parameters:

| Parameter | Type   | Required | Description                                         |
|-----------|--------|----------|-----------------------------------------------------|
| profileId | int    | Yes      | The ID of the user profile                         |
| byDate    | string | No       | Optional filter by specific date (format: yyyy-MM-dd) |
| page      | int    | No       | Page number for pagination (default: 1)            |
| pageSize  | int    | No       | Number of records per page (default: 20)           |

##### Response:

```json
{
  "items": [
    {
      "id": 0,
      "profileId": 0,
      "name": "string",
      "type": 0,
      "status": 0,
      "durationSec": 0,
      "caloriesBurned": 0,
      "finishedAt": "2025-08-20T08:15:23.596Z"
    }
  ],
  "totalCount": 0,
  "page": 0,
  "pageSize": 0
}
```

#### `GET /diaries/fetch`

Fetch a paginated list of diary entries for a specific profile.

##### Request Parameters:

| Parameter | Type   | Required | Description                          |
|-----------|--------|----------|--------------------------------------|
| profileId | int    | Yes      | The ID of the user profile           |
| page      | int    | No       | Page number for pagination (default: 1) |
| pageSize  | int    | No       | Number of records per page (default: 20) |

##### Response:

```json
{
  "items": [
    {
      "id": 0,
      "profileId": 0,
      "title": "string",
      "preview": "string",
      "writtenAt": "2025-08-20T08:17:12.399Z"
    }
  ],
  "totalCount": 0,
  "page": 0,
  "pageSize": 0
}
```

### Column page

#### `GET /columns/fetch`

Fetch a paginated list of published health articles (columns). Articles can be filtered by category such as "Diet", "Beauty", or "Health".

##### Request Parameters:

| Parameter   | Type   | Required | Description                                        |
|-------------|--------|----------|----------------------------------------------------|
| categoryId  | int    | No       | ID of the article category to filter by            |
| page        | int    | No       | Page number for pagination (default: 1)            |
| pageSize    | int    | No       | Number of items per page (default: 20)             |

##### Response:

```json
{
  "items": [
    {
      "id": 0,
      "slug": "string",
      "title": "string",
      "summary": "string",
      "displayImage": "string",
      "isPublished": true,
      "createdAt": "2025-08-20T08:08:55.127Z",
      "publishedAt": "2025-08-20T08:08:55.127Z",
      "taxonomies": [
        {
          "id": 0,
          "name": "string",
          "type": 0
        }
      ]
    }
  ],
  "totalCount": 0,
  "page": 0,
  "pageSize": 0
}
```

#### `GET /columns/slug/{slug}`

Retrieve the full content of a specific article using its unique slug.

##### Query Parameters:

| Parameter | Type   | Required | Description                     |
|-----------|--------|----------|---------------------------------|
| slug      | string | Yes      | The unique slug of the article |

##### Response:

```json
{
  "id": 0,
  "slug": "string",
  "title": "string",
  "summary": "string",
  "content": "string",
  "displayImage": "string",
  "isPublished": true,
  "createdAt": "2025-08-20T08:10:04.895Z",
  "updatedAt": "2025-08-20T08:10:04.895Z",
  "publishedAt": "2025-08-20T08:10:04.895Z",
  "taxonomies": [
    {
      "id": 0,
      "name": "string",
      "type": 0
    }
  ]
}
```

## ▶️ How to Run

You can run the Health Management Backend either using Docker for containerized environments or locally

1. Make sure you have Docker + Docker Compose installed.
2. Navigate to the root directory.
3. Build Docker images:

```bash
docker compose --profile postgres --env-file .\.env.Development build
```

4. Run the application:
- To run everything in containers:
  - Update `appsettings.Development.json` if necessary.
  - Run the following command:
  
```bash
docker compose --profile all --env-file .\.env.Development up -d
```

- To run the backend locally (with services in containers):
  - Update `appsettings.Local.json` if necessary.
  - Start the required services, then launch the application using the Local profile in Visual Studio:
    
```bash
docker compose --profile local --env-file .\.env.Development up -d
```

5. Access the app:
- API: `http://localhost:5292`
- Swagger: `http://localhost:5292/swagger/index.html`

6. Stop and clean up containers:

```bash
docker compose --profile all --env-file .\.env.Development down
```

## **⏱️ Time Spent**

| Phase               | Description                                                                                   | Time Spent |
|--------------------|-----------------------------------------------------------------------------------------------|------------|
| Discovery & Design | Analyzed requirements (Top/My Record/Column pages),  designed API & entities   | 2 hours    |
| Setup             | Initialized solution, configured ASP.NET Core, integrated PostgreSQL + Redis via Docker       | 0.5 hours  |
| Implementation     | Built authentication, profile, body records, exercises, meals, diaries, articles modules      | 6 hours  |
| Testing           | Manual testing of endpoints, verified data flows and validation logic                         | 1 hour     |
| Documentation     | Drafted `README.md`, API docs, technical architecture, and diagrams                          | 3 hours    |

**Total Time: 12.5 hours**

## 📈 Future Improvements

The current implementation serves the core functionalities well, but there are opportunities to improve scalability, maintainability, and robustness:

- Partition data by hot, warm, and cold strategy to optimize performance and reduce storage cost.
- Introduce an API Gateway to handle cross-cutting concerns like routing, rate limiting, logging, and authentication centrally.
- Add load balancer support to improve scalability and ensure high availability under load.
- Separate models for create and update operations to ensure better validation and intent clarity.
- Save article or diary content to a file system or separate content table for easier content management and performance.
- Add stronger validation logic across layers (e.g., type checks, business rules) to improve data integrity and user experience.
- Improve automated test coverage, especially for edge cases and failure scenarios.
- Add metrics and health check endpoints to support monitoring and observability.
- Make configuration (e.g., pagination defaults, API limits) more customizable via app settings or environment variables.
