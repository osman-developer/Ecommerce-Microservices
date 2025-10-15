 
# 🛒 ECommerce Microservices

A modular & production-grade **eCommerce microservices application** built with **.NET 8**, applying a clean microservice architecture, Onion design pattern and following best practices. Each service operates independently and communicates securely through an API Gateway that manages routing, authentication, rate limiting, and reliability policies. The solution emphasizes clean architecture, reusability, and maintainability through a dedicated Common project that provides shared middleware, handlers, helpers, and repository abstractions. It integrates caching, retry strategies, and centralized exception handling to ensure consistent performance and fault tolerance across all services. Extensive unit and integration testing validate every layer, ensuring reliability and stability in distributed environments — making this setup ideal for enterprise-grade, cloud-ready eCommerce systems.

---

## 🧩 Architecture Overview

The solution is divided into several layers and projects:

- **API Gateway** – acts as a single entry point for all requests.  
  Handles routing, rate limiting, caching, retries, and authentication.
- **Independent Microservices** – each focused on a single domain (Authentication, Orders, Products, etc.), communicating via HTTP through the gateway.
- **Common Project (`ECommerce.Common`)** – a shared library providing reusable components like middleware, handlers, helpers, repository interfaces, and response models.
- **Onion Architecture** – separates concerns between Domain, Application, Infrastructure, and API layers for clean code and testability.

---

## ⚙️ Key Features

### 🔒 Security & Middleware
- `AllowOnlyTrustedMiddleware` → allows requests only from trusted internal services using API keys.
- `AddTrustedHeaderHandler` → automatically attaches trusted headers for inter-service communication.
- `JwtValidationParametersHelper` → centralizes JWT token validation configuration.
- `GlobalExceptionHandler` → centralized exception and logging middleware.
  
### ⚡ Reliability & Resilience
- Built-in **retry strategies** for transient network issues.  
- **Caching** and **rate limiting** support in the Gateway to optimize performance.

### 🧱 Shared Abstractions
- `IGenericRepository<T>` → reusable repository pattern across services.  
- `Response<T>` → unified API response model for all endpoints.  
- `SharedServiceExtensions` → extension methods for dependency injection setup.  


---

## 🧠 Common Project Highlights

The **ECommerce.Common** layer promotes clean reusability and consistency:
- Dependency injection helpers for all services.
- Middleware enforcing internal trust policies.
- Shared models,DTOs and utilities to avoid code duplication.
- A foundation for consistent logging and error handling across microservices.

---

## 🧪 Unit Testing

Unit and integration tests are included to validate API endpoints, repository logic, and middleware behavior.

**Tools used:**
- **xUnit** for test organization and assertions.  

These tests ensure each service functions correctly, API routes are valid, and communication between services through the Gateway remains reliable.

---

## 🚀 Getting Started

```bash
git clone https://github.com/osman-developer/ECommerce-Microservices.git
cd ECommerce-Microservices
dotnet build
dotnet run --project ECommerce.Gateway.API
