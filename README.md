# 🎓 Student Course Registration System

A **Student Course Registration System** built with **ASP.NET Core MVC** that allows students to register and manage their courses, while admins can fully manage the course catalog with proper validations.

---

## 📌 Features Overview

### 👤 Student
- Register & Login
- Browse available courses
- Register / Unregister courses
- View registered courses

### 👨‍💼 Admin
- View all courses
- Add new courses
- Edit existing courses
- Delete courses *(only if no students are registered)*

---

## 🛠️ Tech Stack
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Identity

---

## 🏗️ Architecture
This project follows **3-Tier Architecture**:

- **Controllers** → Handle HTTP requests
- **Services** → Business logic & validation
- **Repositories** → Data access layer
- **Database** → SQL Server

---

## 🔐 Authentication & Authorization
- ASP.NET Identity
- Role-based authorization (**Admin / Student**)
- Secure login & registration
- Anti-forgery tokens for all forms

---

## 🎓 Course Registration Flow
1. Student logs in
2. Browses available courses
3. Registers or unregisters from courses
4. Views registered courses in **My Courses**

---

## 🌍 Localization
- Arabic & English support
- Resource files (`.resx`)
- RTL / LTR layout support

---

## 🗄️ Database Design

### Tables
- `AspNetUsers`
- `AspNetRoles`
- `Courses`
- `StudentCourses`

### Relationship
- **Many-to-Many** between Students and Courses  
  Implemented using `StudentCourses` table

---

## ⚙️ Getting Started

### 🔹 Prerequisites
- Visual Studio 2022
- .NET 7 or .NET 8
- SQL Server
- SQL Server Management Studio (SSMS)

---

### 🔹 Clone the Repository
```bash
git clone https://github.com/your-username/student-course-registration.git
cd student-course-registration
