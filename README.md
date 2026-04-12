# Doctor License Management System

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-6.0+-512BD4.svg)](https://dotnet.microsoft.com)
[![C# Version](https://img.shields.io/badge/C%23-10-239120.svg)](https://github.com/topics/csharp)

## 📋 Overview

**Doctor License Management System** is an enterprise-grade ASP.NET Core application designed to streamline the management of medical professional licenses, verifications, and compliance tracking. Built with a scalable Clean Architecture pattern, this system provides healthcare organizations with a robust platform for managing doctor credentials, license renewals, and regulatory compliance.

### Key Capabilities

- **License Management**: Create, update, and track medical licenses across multiple jurisdictions
-- **Verification Workflow**: Comprehensive verification processes for credential validation
- **Audit Logging**: Complete audit trails for regulatory compliance and accountability
- **RESTful API**: Modern API design for seamless integration with third-party systems

---

## 🏗️ Architecture

This project follows **Clean Architecture** principles, ensuring separation of concerns, testability, and maintainability.

### Project Structure

```
DoctorLicenseManagement/
├── DoctorLicenseManagement.API          # Presentation Layer (Controllers, DTOs)
├── DoctorLicenseManagement.Application  # Business Logic & Use Cases
├── DoctorLicenseManagement.Domain       # Core Domain Models & Entities
├── DoctorLicenseManagement.Infrastructure # Data Access & External Services
└── dbscripts/                           # Database initialization scripts
```

### Layered Architecture

| Layer | Responsibility | Technologies |
|-------|---|---|
| **API** | HTTP endpoints, request validation, response formatting | ASP.NET Core, Controllers |
| **Application** | Business rules, orchestration, DTOs | CQRS, MediatR, AutoMapper |
| **Domain** | Entities, value objects, domain logic | C# entities, interfaces |
| **Infrastructure** | Data persistence, external APIs, repositories | Entity Framework Core, SQL Server |

### Design Patterns Implemented

- **Repository Pattern**: Data access abstraction layer
- **Dependency Injection**: Loose coupling and testability
- **CQRS**:  
- **SOLID Principles**: Maintainable and scalable codebase
- **DTOs (Data Transfer Objects)**: API contract separation from domain models

---

## 🚀 Getting Started

### Prerequisites

- **.NET SDK 6.0+** - [Download](https://dotnet.microsoft.com/download)
- **SQL Server 2019+** - Local or cloud instance
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git** for version control

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/tech-naushad/DoctorLicenseManagement.git
   cd DoctorLicenseManagement
   ```

2. **Configure Database Connection**
   - Update `appsettings.json` in the API project:
   ```json
   {
     "ConnectionStrings": {
       "DoctorLicenseConnection": "Server=YOUR_SERVER;Database=DoctorLicense;Trusted_Connection=true;"
     }
   }
   ```

3. **Run Database Migrations**
   ```bash
   cd DoctorLicenseManagement.Infrastructure
   dotnet ef database update
   ```
   Or execute SQL scripts from `/dbscripts` folder directly.

4. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

5. **Build the Solution**
   ```bash
   dotnet build
   ```

6. **Run the Application**
   ```bash
   cd DoctorLicenseManagement.API
   dotnet run
   ```
   The API will be available at `https://localhost:5001/swagger`

---

## 📚 Core Features

### Doctor License Management
- Add, update, and delete doctor licenses
- Track license status (Active, Expired, Suspended)
- Support for multiple licenses per doctor across jurisdictions


---

## 🔌 API Endpoints

### Doctors
```
GET    /api/DoctorsQuery                 # List all doctors
GET    /api/DoctorsQuery/{id}           # Get doctor details
POST   /api/doctors                    # Create new doctor
PUT    /api/doctors/{id}               # Update doctor
DELETE /api/doctors/{id}               # Delete doctor
```

 
---

## 🛠️ Technology Stack

| Category | Technology |
|----------|---|
| **Framework** | ASP.NET Core 6.0+ |
| **Language** | C# 10+ |
| **Database** | SQL Server / SQL Server Express |
| **ORM** | Entity Framework Core 6.0+ |
| **API Documentation** | Swagger/OpenAPI |
| **Dependency Injection** | Built-in .NET DI Container |
| **Logging** | Serilog |
| **Authentication** | JWT Bearer Tokens |

---

## 🧪 Testing

The project is structured to support comprehensive testing across layers.

### Unit Tests
```bash
dotnet test --filter "Category=Unit"
```

### Integration Tests
```bash
dotnet test --filter "Category=Integration"
```

### Running All Tests
```bash
dotnet test
```

---

## 📋 Database Schema

Key tables include:
- **Doctor**: Medical professional information

Database initialization scripts are provided in `/dbscripts` directory.

## 📈 Performance Optimizations

- **Query Optimization**: Efficient Entity Framework queries with eager loading
- **Caching**: In-memory caching for frequently accessed data
- **Pagination**: Implemented on all list endpoints
- **Async Operations**: Fully asynchronous API endpoints for scalability
- **Indexing**: Optimized database indexes on frequently queried columns

---

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Standards
- Follow C# coding conventions (PascalCase for public members)
- Write meaningful commit messages
- Add unit tests for new features
- Update documentation as needed

---

## 📝 License

This project is licensed under the MIT License. See the LICENSE file for details.

---

## 📞 Support & Contact

For issues, feature requests, or questions:
- **GitHub Issues**: [Create an issue](https://github.com/tech-naushad/DoctorLicenseManagement/issues)
- **Email**: [Contact Developer]
- **Documentation**: Check `/docs` folder for detailed guides

---

## 🎯 Future Enhancements

- [ ] Integration with state medical board APIs
- [ ] Multi-language support
- [ ] Mobile application for license tracking
- [ ] Advanced analytics dashboard
- [ ] Automated compliance notifications
- [ ] Document management system enhancement

---

## 📊 Project Statistics

- **Language**: C# (89.7%), T-SQL (10.3%)
- **Architecture**: Clean Architecture with SOLID principles
- **Code Quality**: Production-ready with comprehensive error handling
- **Maintainability**: Modular design with clear separation of concerns

---

## ✨ Key Achievements

✅ Scalable Clean Architecture implementation  
✅ RESTful API with comprehensive endpoints  
✅ Role-based security and compliance features  
✅ Database-driven design with proper normalization  
✅ Complete audit logging and traceability  
✅ API documentation with Swagger  
✅ Production-ready error handling and validation  

---

## 📚 Additional Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

---

**Version**: 1.0.0  
**Last Updated**: April 2026  
**Author**: Naushad

---

*This project demonstrates professional software engineering practices suitable for healthcare and regulated industry applications.*
