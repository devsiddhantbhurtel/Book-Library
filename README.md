# 📚 Book Library Management System

A full-featured **Book Library System** built with ASP.NET Core and Razor Pages. It supports user accounts, book management, borrowing, cart and order functionalities, and staff/admin tools — all in one platform.

---

## 🔧 Tech Stack

- **Backend:** ASP.NET Core MVC
- **Frontend:** Razor Pages, Bootstrap, jQuery
- **Database:** MySQL (via MySqlConnector)
- **Authentication:** JWT & Cookie-based
- **Real-Time Features:** SignalR (Staff Notifications)
- **State Management:** Session-based auth with middleware
- **ORM:** Entity Framework Core
- **GitHub Actions:** CI-ready structure
- **Cloud Ready:** Can be hosted on IIS, Azure, or Dockerized

---

## ✨ Key Features

### 👤 User Features

- 🔐 Secure Registration & Login (JWT-based)
- 📖 Browse & Search Books by Genre, Author, Publisher
- 📥 Add Books to Cart
- 🛒 Place Orders
- 🔖 Bookmark Favorites
- 📚 View Personal Borrowing History
- 📝 Leave Reviews & Ratings
- 👤 Update Profile

---

### 🔧 Admin Features

- 👥 Manage Users & Staff
- 📘 CRUD for Books, Authors, Genres, Publishers
- 🧾 Manage Book Discounts
- 📣 Publish Announcements
- 🏆 Assign Book Awards
- 📊 Admin Dashboard for Analytics
- 📥 View and Manage All Orders

---

### 🧑‍💼 Staff Features

- 🖊️ Claim Book Orders for Processing
- ⏱ Track Claim Time
- 📥 View Assigned Tasks
- 🔔 Receive Real-Time Notifications via SignalR
- 🗂 Access Staff Dashboard

---

### 📢 Announcement System

- 📣 Add, Edit, and Remove Announcements (Admin)
- 📰 Display Active Announcements on Home Page

---

### 🧰 Extra Functionalities

- 📅 Filter Books
- 🔍 Search Functionality
- 🎨 Responsive UI with Bootstrap
- 🎁 Discounts with Types (Percentage, Fixed)
- 📂 Upload Book Cover Images
- 📑 Razor Pages + MVC Controllers Mixed Architecture

---

## ⚙️ Configuration

Update the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=booklibrarydb;user=root;password=yourpassword;"
  },
  "JwtSettings": {
    "SecretKey": "your-very-secret-key",
    "ExpiryMinutes": 60
  }
}
