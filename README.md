# 🛍️ OnlineShop-RazorPages

A modular and clean **e-commerce web application** built with **C#**, **ASP.NET Core Razor Pages**, and **SQL Server** using **Onion Architecture**.

---

## 🚀 Features

- ✅ User registration and login  
- 🛒 Product listing and search  
- 🧺 Shopping cart and checkout  
- 🔐 Basic user authentication  
- 🛠️ Admin panel for managing products  
- 🧱 Onion Architecture  
- 🗃️ SQL Server integration using EF Core  
- 🎨 Basic UI with HTML & CSS  

---

## 🧰 Technologies Used

- `C#`
- `ASP.NET Core Razor Pages`
- `Entity Framework Core`
- `SQL Server`
- `HTML5 & CSS3`
- `Onion Architecture`

---

## 📦 Project Contexts

The project includes the following **bounded contexts**, each structured independently using Onion Architecture:

- `Shop`
- `Account`
- `Blog`
- `Comment`
- `Discount`
- `Inventory`

Each context has:

```plaintext
Domain/
Application/
Application.Contracts/
Infrastructure/


## ⚙️ Getting Started
Run with Visual Studio
Open the solution in Visual Studio.

Go to: Tools > NuGet Package Manager > Package Manager Console.

In the Package Manager Console, set the Default Project to the corresponding Infrastructure.EFCore layer of a context.

Run the following command to apply migrations and create the database:

Update-Database -Context YourContextName
