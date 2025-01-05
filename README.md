# Talabat - E-commerce Web API  

## Overview  
**Talabat** is a modern e-commerce web application providing advanced features for product exploration, filtering by brands and categories, cart management, order creation, and payment processing. The project is built using **ASP.NET Web API** with a scalable and flexible architecture.  

---

## Features  
- **Product Browsing:**  
  Explore all products with search and filtering by brands and categories.  
- **Cart Management:**  
  Integrated shopping cart using Redis for in-memory storage.  
- **Order Creation:**  
  Seamless order creation for users.  
- **Payment Integration:**  
  Secure and reliable payment processing via Stripe integration.  
- **API Documentation:**  
  Comprehensive API documentation with Swagger.  
- **User Security:**  
  User authentication and authorization using Identity with JWT.  

---

## Technologies Used  
- **ASP.NET Web API**  
- **Onion Architecture**  
- **AutoMapper** - For mapping between models and DTOs  
- **Identity & JWT** - For authentication and authorization  
- **Redis** - For in-memory cart storage  
- **Stripe** - For payment integration  
- **Swagger** - For API documentation  
- **CORS Policy** - For deployment on IIS  

---

## Key Design Patterns  
- **Dependency Injection**  
- **Generic Repository Pattern**  
- **Unit of Work**  
- **Specification Pattern**  

---

## Installation  
1. Clone the repository to your local machine:  
   ```bash
   git clone https://github.com/Abdallah-Shatta/talabat-api.git

2. Open the project in Visual Studio.


3. Configure the database connection in the appsettings.json file.


4. Run migrations to create the database:
    ```bash
    dotnet ef database update

5. Start the project.

---

## Contact
- **Email:** [abdallah_shatta@outlook.com](mailto:your-email@example.com)  
- **LinkedIn:** [Abdallah-Shatta](https://www.linkedin.com/in/abdallah-shatta55/)
