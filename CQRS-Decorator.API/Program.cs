using CQRS_Decorator.API;
using CQRS_Decorator.API.Extensions;
using CQRS_Decorator.API.Middlewares;
using CQRS_Decorator.Infrastructure.Data;
using CQRS_Decorator.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use fixed ports
builder.WebHost.UseUrls("https://localhost:7036", "http://localhost:5246");

// Add services to the container.
string corsPolicy = "CorsPolicy";

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();
builder.Services.AddCorsInApplication(corsPolicy);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.InitializeServices(builder.Configuration);  
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

// Add data seeders
builder.Services.AddDataSeeders();



var app = builder.Build();

// Apply migrations and seed data automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        // Check if database exists
        var canConnect = await context.Database.CanConnectAsync();
        
        if (!canConnect)
        {
            logger.LogInformation("Database does not exist. Creating database and applying migrations...");
            
            // Apply migrations to create database
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully");

            // Seed data only for new database
            var seeder = services.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();
            logger.LogInformation("Database seeding completed successfully");
        }
        else
        {
            logger.LogInformation("Database already exists. Checking for pending migrations...");
            
            // Check if there are pending migrations
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} pending migration(s)...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Pending migrations applied successfully");
            }
            else
            {
                logger.LogInformation("Database is up to date. No migrations needed.");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
