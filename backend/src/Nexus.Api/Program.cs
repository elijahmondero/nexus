using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nexus.Api.Auth;
using Nexus.Api.Data;
using Nexus.Api.Data.Migrations;
using Nexus.Api.Middleware;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// 1. Config
var jwtSettings = new JwtSettings();
jwtSettings.Secret = builder.Configuration["JWT_SECRET"] ?? builder.Configuration["JWT:Secret"] ?? "super_secret_test_key_32_characters_long";
jwtSettings.Issuer = builder.Configuration["JWT_ISSUER"] ?? builder.Configuration["JWT:Issuer"] ?? "nexus-api";
jwtSettings.Audience = builder.Configuration["JWT_AUDIENCE"] ?? builder.Configuration["JWT:Audience"] ?? "nexus-client";

builder.Services.AddSingleton(jwtSettings);



// 2. Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexus API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddScoped<AuthService>();

// 3. Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Use a local variable to ensure we don't capture the wrong state if it changes
        var currentSettings = jwtSettings;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = currentSettings.Issuer,
            ValidAudience = currentSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(currentSettings.Secret))
        };
    });

builder.Services.AddAuthorization();

// 4. OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddSource("Nexus.Api")
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Configuration["OTEL_SERVICE_NAME"] ?? "nexus-api"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri(builder.Configuration["OTEL_EXPORTER_ZIPKIN_ENDPOINT"] ?? "http://localhost:9411/api/v2/spans");
            });
    });

var app = builder.Build();

// 6. Migrations
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<DbConnectionFactory>();
    MigrationRunner.Run(factory.ConnectionString, factory.Provider);
}

// 7. Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
