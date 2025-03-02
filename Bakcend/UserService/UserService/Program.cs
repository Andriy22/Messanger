using System.Reflection;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Persistence;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using UserService.Extensions;
using UserService.Middleware;
using UserService.Options;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .WriteTo.File("WebAppLog-.txt", rollingInterval:
                  RollingInterval.Day)
              .CreateLogger();


builder.Services.AddOpenApi();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(IDataContext).Assembly);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Host.UseSerilog(logger);


var app = builder.Build();

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapEndponts();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception)
    {
        throw;
    }
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.Run();
