
# 📚 BetterReads
BetterReads is a modular, microservices-based application designed to enhance your reading experience. It allows users to manage bookshelves, write reviews, receive personalized recommendations, and more.
It is a personal project to improve tech skills.

---

## 🛠️ Technologies Used
- ASP.NET Core: Web framework for building APIs.

- Entity Framework Core: Object-relational mapper for database interactions.

- Docker: Containerization of services for easy deployment.

- Kubernetes: Deployment, orchestrates.

- Azure Pipelines: CI/CD pipeline for automated builds and deployments.

- Redis: In-memory data store used for caching and message brokering.
- Azure Key Vault: Storing secrets/keys
- AWS Cognito: Managing user authentication and authorization
- Ocelot: API Gateway
- MediatR: CQRS
- MongoDB: Storing data
- Azure Service Bus - For communication between decoupled services using asynchronous messaging
- Azure OpenAI - Getting current book recommendations
- Github Actions: CI/CD pipeline for automated builds
- MassTransit: Messaging, sagas
- Outbox pattern: At least once delivery

---

## 🚀 Features
- Bookshelf Management: Organize your reading list with custom shelves.

- Book Reviews: Share your thoughts and read others' opinions.

- Personalized Recommendations: Discover books tailored to your interests.

- User Authentication: Secure sign-up and login functionality.

- API Gateway: Centralized entry point for all services.

- Microservices Architecture: Each feature is a separate, scalable service.

---

## 🧱 Project Structure
The solution is divided into several projects:

- BetterReads.ApiGateway: Handles routing and acts as the entry point for all services.

- BetterReads.Auth: Manages user authentication and authorization.

- BetterReads.Books: Handles book-related data and operations.

- BetterReads.Reviews: Manages user reviews for books.

- BetterReads.Recommendations: Provides book recommendations based on user activity.

- BetterReads.Shelves: Manages user-created bookshelves.

- BetterReads.Shared: Contains shared models and utilities used across services.
