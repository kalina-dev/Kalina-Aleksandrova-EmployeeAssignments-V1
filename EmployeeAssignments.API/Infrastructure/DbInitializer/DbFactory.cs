using System.Data;
using Dapper;
using EmployeeAssignments.API.Entities;
using EmployeeAssignments.API.Infrastructure.Connection;
using EmployeeAssignments.API.Infrastructure.DataSeeder;

namespace EmployeeAssignments.API.Infrastructure.DbInitializer;

public class DbFactory(IDbConnectionFactory db, IDbDataSeeder generator) : IDbFactory
{
    private readonly IDbConnectionFactory db = db;
    private readonly IDbDataSeeder generator = generator;

    public async Task<IDbFactory> EnsureTablesCreationAsync()
    {
        using (var con = await db.OpenConnectionAsync())
        {
            string createProjectsTable = @"
            CREATE TABLE IF NOT EXISTS Projects (
                ProjectID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL CHECK(length(Name) >= 3)
            );
            ";

            string createEmployeesTable = @"
            CREATE TABLE IF NOT EXISTS Employees (
                EmpID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL CHECK(length(Name) >= 3)
            );
            ";

            string createEmployeeProjectsTable = @"
            CREATE TABLE IF NOT EXISTS EmployeeProjects (
                EmpID INTEGER NOT NULL,
                ProjectID INTEGER NOT NULL,
                DateFrom TEXT NOT NULL DEFAULT (datetime('now')),
                DateTo TEXT,
                PRIMARY KEY (EmpID, ProjectID, DateFrom),
                FOREIGN KEY (EmpID) REFERENCES Employees(EmpID) ON DELETE CASCADE,
                FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE
            );
            ";

            // Execute table creation
            await con.ExecuteAsync(createProjectsTable);
            await con.ExecuteAsync(createEmployeesTable);
            await con.ExecuteAsync(createEmployeeProjectsTable);
        }

        return this;
    }

    public async Task SeedAsync()
    {
        using var con = await db.OpenConnectionAsync();
        await ClearDatabaseAsync(con);
        var (employees, projects) = generator.GenerateData(20, 20);

        string insertProjectsSql = @"INSERT INTO Projects (Name) VALUES (@Name)";

        string insertEmployeesSql = @"INSERT INTO Employees (Name) VALUES (@Name)";

        string insertEmployeeProjectsSql = "INSERT INTO EmployeeProjects(EmpID, ProjectID, DateFrom, DateTo) VALUES(@EmpID, @ProjectID, @DateFrom, @DateTo)";

        await con.ExecuteAsync(insertProjectsSql, projects);
        await con.ExecuteAsync(insertEmployeesSql, employees);

        var insertedProjects = (await con.QueryAsync<Project>("SELECT * FROM Projects")).ToList();
        var insertedEmployees = (await con.QueryAsync<Employee>("SELECT * FROM Employees")).ToList();

        var employeeProjects = generator.GenerateData(20, insertedEmployees, insertedProjects);

        await con.ExecuteAsync(insertEmployeeProjectsSql, employeeProjects);
    }

    private static async Task ClearDatabaseAsync(IDbConnection con)
    {
        // Disable foreign key constraints temporarily
        await con.ExecuteAsync("PRAGMA foreign_keys = OFF;");

        await con.ExecuteAsync("DELETE FROM Employees;");
        await con.ExecuteAsync("DELETE FROM Projects;");
        await con.ExecuteAsync("DELETE FROM EmployeeProjects;");

        // Re-enable foreign key constraints
        await con.ExecuteAsync("PRAGMA foreign_keys = ON;");
    }
}