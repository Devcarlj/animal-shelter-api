# 🐾 Animal Shelter Adoption & Foster Placement Tracker

A robust, relational .NET 8 ASP.NET Core Web API built using a **Database-First** approach[cite: 5]. It manages essential shelter operations, including animal intake tracking, adopter registration, and adoption application processing, while enforcing automated business rules and secure data binding through backend logic[cite: 5].

---

## 🚀 Setup & Installation Guide 
Follow these steps to clone and run the project locally on your machine:

### 1. Clone the Repository
Open your terminal or Git Bash and clone the project to your local machine:
```bash
git clone https://github.com/Devcarlj/animal-shelter-api.git
cd AnimalShelterApi

```

### 2. Restore NuGet Packages

Because `bin/` and `obj/` folders are ignored by Git, you need to restore the required NuGet dependencies. Run this command in your project root or Package Manager Console:

```bash
dotnet restore

```

*(Alternatively, if using Visual Studio, simply open the solution file, right-click the project, and select **Restore NuGet Packages**).*

### 3. Set Up the Database in SQL Server

1. Open **SQL Server Management Studio (SSMS)**.
2. Run the following SQL script to create your local database, set up relational tables, and pre-seed sample data:

```sql
CREATE DATABASE AnimalShelterDb;
GO

USE AnimalShelterDb;
GO

-- 1. Animals Table
CREATE TABLE Animals (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Species NVARCHAR(50) NOT NULL, -- e.g., Dog, Cat
    Breed NVARCHAR(100) NOT NULL,
    AgeMonths INT NOT NULL,
    HealthStatus NVARCHAR(100) NOT NULL, -- e.g., Vaccinated, Healthy, Under Treatment
    IsAdoptable BIT NOT NULL DEFAULT 1 -- 1 = Available, 0 = Adopted/Locked
);
GO

-- 2. Adopters Table
CREATE TABLE Adopters (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL,
    IsBlacklisted BIT NOT NULL DEFAULT 0 -- 0 = Eligible, 1 = Banned
);
GO

-- 3. Adoption Applications Table (The Relational Transaction)
CREATE TABLE AdoptionApplications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnimalId INT NOT NULL,
    AdopterId INT NOT NULL,
    ApplicationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    CONSTRAINT FK_Applications_Animals FOREIGN KEY (AnimalId) REFERENCES Animals(Id),
    CONSTRAINT FK_Applications_Adopters FOREIGN KEY (AdopterId) REFERENCES Adopters(Id)
);
GO

-- 4. Foster Placements Table (Temporary Adoption)
CCREATE TABLE FosterPlacements (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AnimalId INT NOT NULL,
    AdopterId INT NOT NULL,
    TermType NVARCHAR(50) NOT NULL, -- 'Trial', 'Monthly', 'Quarterly', 'Custom'
    StartDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    EndDate DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active', -- 'Active', 'Completed', 'Cancelled'
    CONSTRAINT FK_Foster_Animals FOREIGN KEY (AnimalId) REFERENCES Animals(Id),
    CONSTRAINT FK_Foster_Adopters FOREIGN KEY (AdopterId) REFERENCES Adopters(Id)
);
GO

-- Seed sample data
INSERT INTO Animals (Name, Species, Breed, AgeMonths, HealthStatus, IsAdoptable) 
VALUES ('Max', 'Dog', 'Golden Retriever', 24, 'Vaccinated', 1),
       ('Luna', 'Cat', 'Domestic Shorthair', 12, 'Healthy', 1);

INSERT INTO Adopters (FullName, Email, PhoneNumber, IsBlacklisted) 
VALUES ('Sarah Connor', 'sarah@example.com', '09123456789', 0);
GO

```

3. Verify that your connection string in **`appsettings.json`** matches your local SQL Server instance:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=AnimalShelterDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

```



### 4. Run the API

* Press **F5** in Visual Studio or run this command in your terminal:
```bash
dotnet run

```


* The **Swagger UI** will automatically launch in your browser (`https://localhost:xxxx/swagger`) so you can test all endpoints.

---

## 🌿 Git Workflow & Branching Guidelines

To keep our codebase organized and prevent merge conflicts, please follow this branch workflow when adding new features:

### 1. Create a New Feature Branch

Never push code directly to the `main` branch. Before writing code, create a dedicated branch named after your feature:

```bash
git checkout -b feature/your-feature-name

```

*(Example: `git checkout -b feature/medical-records`)*

### 2. Commit Your Changes Regularly

As you build your feature, stage and commit your changes with clear messages:

```bash
git add .
git commit -m "Add medical records CRUD endpoints and business rules"

```

### 3. Push and Open a Pull Request

Push your branch to GitHub:

```bash
git push origin feature/your-feature-name

```

Go to GitHub, open a **Pull Request (PR)** from your feature branch into `main`, and tag a teammate for review!

---

