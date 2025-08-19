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
