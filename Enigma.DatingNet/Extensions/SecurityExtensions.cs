using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Enigma.DatingNet.Extensions;

public static class SecurityExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };
            });
    }

    public static void AddJwtSwaggerAuth(this IServiceCollection services)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JSON Web Token based security",
        };
        
        var securityReq = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };
        
        var contact = new OpenApiContact
        {
            Name = "Rifqi Ramadhan",
            Email = "rifqyomp@gmail.com",
            Url = new Uri("https://github.com/rifqyram")
        };
        
        var license = new OpenApiLicense
        {
            Name = "Free License",
            Url = new Uri("https://github.com/rifqyram")
        };
        
        var info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Minimal API - JWT Authentication with Swagger demo",
            Description = "Implementing JWT Authentication in Minimal API",
            Contact = contact,
            License = license
        };
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", info);
            o.AddSecurityDefinition("Bearer", securityScheme);
            o.AddSecurityRequirement(securityReq);
        });
    }
}