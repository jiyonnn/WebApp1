using App1.Data;
using App1.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<App1DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExpenseService, ExpenseService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseStartup");
    var dbContext = services.GetRequiredService<App1DbContext>();
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    try
    {
        logger.LogInformation("Connecting to SQL Server with database {Database}.", dbContext.Database.GetDbConnection().Database);

        dbContext.Database.Migrate();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT COUNT(*)
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Expenses'
            """;

        var expenseTableExists = (int)(await command.ExecuteScalarAsync() ?? 0) > 0;

        logger.LogInformation(
            "Database ready on {Server}/{Database}. Table [dbo].[Expenses] exists: {Exists}",
            connection.DataSource,
            connection.Database,
            expenseTableExists);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database startup failed. Check that SQL Server is reachable and the configured database is correct.");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
