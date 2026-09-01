# Video Project Manager

A C# WPF MVVM application for managing video projects and files with MySQL database integration.

## Features

- **Project Management**: Create, edit, and delete projects
- **Video File Management**: Add, edit, and remove video files from projects
- **Batch Upload**: Add multiple video files from a folder at once
- **File Hashing**: Automatic MD5 hash calculation for video files
- **Database Integration**: MySQL backend with Entity Framework Core
- **MVVM Pattern**: Clean separation of concerns with MVVM Toolkit

## Prerequisites

- .NET 6.0 or higher
- MySQL Server
- Visual Studio 2022 or Visual Studio Code

## Setup

### 1. Database Configuration

Update the connection string in `App.xaml.cs`:

```csharp
const string connectionString = "Server=localhost;Port=3306;Database=projects;Uid=root;Pwd=your_password;";
```

### 2. Create Database

Run the provided SQL script to create the database and tables:

```sql
CREATE DATABASE IF NOT EXISTS projects;
USE projects;

-- ... (paste the SQL schema provided)
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run Application

```bash
dotnet run
```

## Project Structure

```
VideoProjectManager/
├── Models/              # Data models (Project, VideoFile)
├── Data/               # DbContext and database configuration
├── Services/           # Business logic (ProjectService, VideoFileService)
├── ViewModels/         # MVVM ViewModels
├── Views/              # WPF Views (XAML)
├── App.xaml(.cs)       # Application configuration and DI setup
└── appsettings.json    # Configuration file
```

## Usage

### Managing Projects

1. Click "Load" to fetch all projects from the database
2. Enter a project title and description
3. Click "Create" to add a new project
4. Select a project and click "Update" to modify it
5. Select a project and click "Delete" to remove it

### Managing Video Files

1. Select a project
2. Add individual video files or batch upload from a folder
3. Edit or delete video files as needed

## Technologies

- **Framework**: WPF with .NET 6.0
- **ORM**: Entity Framework Core 7.0
- **Database**: MySQL with Pomelo Provider
- **MVVM**: CommunityToolkit.Mvvm
- **DI**: Microsoft.Extensions.DependencyInjection

## License

MIT