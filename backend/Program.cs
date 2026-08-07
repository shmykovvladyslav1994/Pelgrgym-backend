using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DotNetEnv;

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    Env.Load();
}

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// JWT секрет
var jwtSecret = builder.Configuration["Jwt:Secret"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new Exception("JWT Secret is not configured");
}

// Аутентификация
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Авторизация
builder.Services.AddAuthorization();
    
// Add services to the container.
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "https://pelgrgym-frontend.onrender.com"
                ).AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials();
        });
});

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

var connectionString =
    $"Host={Environment.GetEnvironmentVariable("DbHost")};" +
    $"Database={Environment.GetEnvironmentVariable("DbName")};" +
    $"Username={Environment.GetEnvironmentVariable("DbUser")};" +
    $"Password={Environment.GetEnvironmentVariable("DbPassword")};" +
    $"SSLMode=Require;TrustServerCertificate=true";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        Console.WriteLine(error?.Error);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal Server Error");
    });
});
app.Urls.Add($"http://*:{port}");
app.UseCors("AllowReactApp"); // <-- включаем CORS

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
