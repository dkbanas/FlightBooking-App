# ✈️ **AirFlyerNET – Airline Ticket Reservation System**
> *This application is my project made in the process of learning .NET and Angular*
# 📌Overview

AirFlyerNET is a web-based airline ticket reservation system designed to provide users with a seamless booking experience. The application consists of a robust backend built with **ASP.NET 9**, a dynamic frontend developed in **Angular 18.2.11**, and a **PostgreSQL 17.0** database for data storage.

# 🚀Key Features

✔️ **User Authentication** – Secure login and registration with role-based access.  
✔️ **JWT-Based Security** – Ensures safe communication between frontend and backend.  
✔️ **Flight Search & Booking** – Users can search, filter, and reserve flights.  
✔️ **Seat Selection** – Interactive seat selection during booking.  
✔️ **Reservation Management** – View and cancel existing reservations.  
✔️ **Admin Dashboard** – Manage flights and airports.  
✔️ **Pagination & Sorting** – Efficient data handling for improved performance.

# 🖥️Backend Architecture

The backend follows the **Clean Architecture** pattern and is structured into the following layers:

📂 **Domain** – Contains entity definitions.  
📂 **Application** – Includes DTOs, interfaces, mappers, and helper functions.  
📂 **Infrastructure** – Manages services, data seeders, database migrations, and seed data stored in JSON format.  
📂 **Presentation** – Manages controllers, extensions, and static assets such as images.  
📂 **Shared** – Contains pagination and sorting models.

### **🔧 Core Services**

1️⃣ **AuthService** – Handles user authentication, registration, role management, and JWT-based security.  
2️⃣ **AirportService** – Manages airport creation, deletion, retrieval, and search operations.  
3️⃣ **FlightService** – Responsible for flight creation, deletion, retrieval, and search.  
4️⃣ **ReservationService** – Manages flight reservations, including booking, cancellation, and retrieval.

# 🎨Frontend Structure

The frontend, developed in **Angular 18.2.11**, is designed for an intuitive and smooth user experience. It consists of:

- **📄 Seven main pages** forming the core user journey.
- **🗂️ Five modals** used for additional interactions.
- **🛠️ Five partial components** ensuring code reusability and modularity.

## 📄Main Pages

1. **Home Page** – A flight search interface with travel tips for cost-effective booking.
2. **Flight Results Page** – Displays available flights based on user criteria.
3. **Flight Details Page** – Shows flight details, allows seat selection, and enables booking.
4. **Reservation Confirmation Page** – Displays booking details after a successful reservation.
5. **Airports Page** – Lists all available airports within the application.
6. **My Reservations Page** – Allows users to view and manage their reservations.
7. **Management Panel (Admin Only)** – Provides administrators with tools to manage airports and flights.

## 💬Modals

1. **Login** - Allows users to authenticate and access their accounts.
2. **Register** - Enables new users to sign up and create an account.
3. **Cancel Reservation** - Confirms reservation cancellation before finalizing the action.
4. **Add airport (Admin Only)** - Provides a form for administrators to add new airports.
5. **Add flight (Admin Only)** - Allows administrators to create and manage new flights.

## 🛠️Partial components

1. **Flight Search Component** - Allows users to enter search criteria and find available flights.
2. **Navbar Component** - Provides easy navigation between pages and adapts based on user roles.
3. **Spinner Component** - Displays a loading animation when data is being fetched.
4. **Tips Component** - Offers travel advice on booking affordable flights.
5. **Cheapest Flight Component** - Display 5 cheapest offers in the app.

# 🔮Future Enhancements

✅ **Integration with PayPal** – Seamless payment processing.  
✅ **Admin Statistics Dashboard** – Insights into flight trends, revenues, and user activity.  
✅ **User Page** – Update of personal information.  
✅ **Unit Testing** – Ensuring application reliability.  
✅ **Integration Testing** – Improving overall system stability.  

# **🧰 NuGet Addons**

### **📌 Presentation Layer**

- `Microsoft.AspNetCore.Authentication.JwtBearer` **9.0.2**
- `Microsoft.AspNetCore.OpenApi` **9.0.2**
- `Microsoft.EntityFrameworkCore.Tools` **9.0.1**
- `Swashbuckle.AspNetCore` **7.2.0**

### **📌 Infrastructure Layer**

- `Microsoft.EntityFrameworkCore` **9.0.1**
- `System.IdentityModel.Tokens.Jwt` **8.4.0**
- `BCrypt.Net-Next` **4.0.3**
- `Npgsql.EntityFrameworkCore.PostgreSQL` **9.0.3**

# 🖼️Screenshots
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/Homepage.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/SearchResults.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/FlightDetails.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/Confirmation.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/UserReservations.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/ManagementPage.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/Airportspage.png)
![image alt](https://github.com/dkbanas/BookingFlight-App/blob/master/Screenshots/LoginModal.png)


# 🛠️How to run

1. Clone repo.
2. Create "airflyerNET" database in pgadmin4 and import sql file from infrastructure folder.
3. Check if appsettings.Development.json is correct.
4. Click run in your IDE to run backend.
5. In Client folder, open terminal and write  "ng serve" to run frontend.
