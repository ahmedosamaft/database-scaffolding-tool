# Database Scaffolding Tool

## Introduction

The Database Scaffolding Tool is a versatile console application designed to streamline the process of generating database tables and corresponding CRUD (Create, Read, Update, Delete) controllers based on a database schema. This documentation provides an overview of the application's features, usage instructions, and technical details.

## Features

- **Dynamic Scaffolding:** Automatically generates database tables and classes based on schema information retrieved from supported database providers.
- **Multiple Database Providers:** Supports SQL Server, MySQL, and **(still not implemented)** PostgreSQL database providers, providing flexibility for developers working with different database systems.
- **CRUD Controller Generation:** Customizable generation of CRUD controllers for efficient management of database entities, reducing boilerplate code and development time.
- **Console Interface:** Utilizes Spectre.Console for an enhanced console interface, providing a user-friendly experience with styling and formatting options.

## Installation

1. Clone the repository: 
```
git clone https://github.com/yourusername/database-scaffolding-tool.git
cd database-scaffolding-tool
```
2. Build the project using Visual Studio or .NET CLI: 
```
dotnet build
```

## Usage

1. Run the application using Visual Studio or .NET CLI: 
```
dotnet run
```
2. Follow the on-screen prompts to generate tables and controllers based on your database schema.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/git/git-scm.com/blob/main/MIT-LICENSE.txt) file for details.

