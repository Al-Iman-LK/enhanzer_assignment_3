# Enhanzer Assignment 3 - Login & Purchase Bill Application

A 2-page Angular + .NET Core application with login authentication and purchase bill management, integrating with external APIs for authentication and including real-time calculations, form validation, and data persistence.

## 🏗️ Project Structure
```
Enhanzer_assignment_3/
├── backend/               # .NET Core Web API
│   ├── Controllers/       # API Controllers
│   ├── Models/           # Data Models
│   ├── Data/             # Database Context
│   └── Program.cs        # Application Entry Point
├── frontend/             # Angular Application
│   └── src/app/
│       ├── login/        # Login Component
│       ├── purchase-bill/ # Purchase Bill Component
│       └── services/     # Angular Services
└── README.md
```

## ✨ Features
- **🔐 Authentication**: External API integration with Enhanzer staging API
- **📋 Purchase Bill Management**: Create, read, update, delete purchase bills
- **💰 Real-time Calculations**: Automatic total cost and selling price calculations
- **✅ Form Validation**: Comprehensive client-side validation
- **🗄️ Data Persistence**: Entity Framework with in-memory database
- **🌐 CORS Support**: Cross-origin requests between frontend and backend
- **📱 Responsive Design**: Modern, clean UI

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK
- Node.js (v18 or higher)
- Angular CLI

### Backend Setup
```bash
cd backend
dotnet restore
dotnet run
```
Backend runs on: `http://localhost:5045`

### Frontend Setup
```bash
cd frontend
npm install
ng serve --port 4201
```
Frontend runs on: `http://localhost:4201`

## 🔑 Test Credentials
- **Email**: info@enhanzer.com
- **Password**: Welcome#3

## 🛠️ Technology Stack
- **Frontend**: Angular 19, TypeScript, CSS
- **Backend**: .NET Core 9.0, ASP.NET Core Web API
- **Database**: Entity Framework Core (In-Memory)
- **External API**: Enhanzer Staging API

## Technologies
- Frontend: Angular, HTML5, CSS3, JavaScript
- Backend: .NET Core, C#
- Database: SQL Server

## Setup Instructions
1. Backend: Navigate to backend folder and run `dotnet run`
2. Frontend: Navigate to frontend folder and run `ng serve`
