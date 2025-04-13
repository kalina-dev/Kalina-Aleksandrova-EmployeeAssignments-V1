using System.Data;
using Microsoft.Data.Sqlite;

namespace EmployeeAssignments.API.Infrastructure.Connection;

public class DbConnectionFactory(IConfiguration Configuration) : IDbConnectionFactory
{
    private string connectionString = Configuration["ConnectionString"];

    public async Task<IDbConnection> OpenConnectionAsync()
    {
        var con = new SqliteConnection(connectionString);
        await con.OpenAsync();
        return con;
    }
}