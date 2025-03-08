
---

# **RealEstate-CleanArchitecture**

A **Real Estate Management System** built with **ASP.NET Core** and **Blazor** following **Clean Architecture** principles. This project features both an **Admin Panel** and a **User Interface (UI)**, providing users and administrators with tools to manage estate data efficiently.

### **Project Overview**
This system allows the management of real estate listings, including property details, sales, and improvements. The system is designed using **Clean Architecture** to ensure flexibility, maintainability, and scalability.

Key technologies:
- **Backend**: ASP.NET Core Web API
- **Frontend**: Blazor (for Admin Panel and User Interface)
- **Database**: Entity Framework Core
- **UI Components**: Radzen UI components for Blazor

### **Features**
- **Admin Panel**: Manage estates, properties, and sales types.
- **User Interface**: Display estate data for users to browse.
- **CRUD Operations**: Create, Read, Update, and Delete estate listings.
- **Authentication**: Admin panel with authentication for restricted access.
- **Responsive Design**: Built with **Radzen Components** for a modern look and feel.
- **Separation of Concerns**: Clean Architecture with clear separation between layers (UI, API, Domain, Infrastructure).

---

### **Installation & Setup**

#### **Prerequisites**
Ensure you have the following installed:
- [**.NET 6.0 or later**](https://dotnet.microsoft.com/download)
- [**Visual Studio**](https://visualstudio.microsoft.com/) or [**VS Code**](https://code.visualstudio.com/) with C# extension
- [**SQL Server**](https://www.microsoft.com/en-us/sql-server) (or any compatible database)

#### **Clone the Repository**

Start by cloning the project repository to your local machine:

```bash
git clone https://github.com/melikaDaa/RealEstate-CleanArchitecture.git
cd RealEstate-CleanArchitecture
```

#### **Set Up the Database**

1. Update the connection string in `appsettings.json` to match your local database setup.

   Example:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RealEstateDb;Trusted_Connection=True;"
   }
   ```

2. Apply the database migrations:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

#### **Run the Project**

Once you've set up the database, you can run the project using the following command:

```bash
dotnet run
```

This will start the application on `https://localhost:5001`.

---

### **Project Structure**

The project is divided into several key folders to separate concerns:

```
├── AdminPanel/            # Code for the Admin Panel UI (Blazor)
├── BlazorUI/              # Code for the User Interface (Blazor)
├── WebApi/                # Code for the Web API (Backend)
├── Data/                  # Database models and context classes
├── Migrations/            # Entity Framework Core migrations
├── README.md              # Project documentation
└── appsettings.json       # Configuration settings (e.g., connection strings)
```

---

### **Usage**

- **Admin Panel**: Access the admin panel at `https://localhost:5001/admin`. Here, you can add and manage estate properties, view improvements, and manage sales types.
  
- **User Interface**: The main user interface is accessible at the root URL (`https://localhost:5001`). Users can view available properties and filter them based on different criteria.


---

### **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

### **Contact**

For any inquiries, issues, or suggestions, please feel free to open an issue on the [GitHub Issues page](https://github.com/melikaDaa/RealEstate-CleanArchitecture/issues). You can also contact me directly at 

