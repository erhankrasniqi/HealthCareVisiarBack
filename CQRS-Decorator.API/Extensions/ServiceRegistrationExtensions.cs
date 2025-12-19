using CQRS_Decorator.Application.Features.Commands.BookAppointment;
using CQRS_Decorator.Application.Features.Commands.CancelAppointment;
using CQRS_Decorator.Application.Features.Commands.LoginPatient;
using CQRS_Decorator.Application.Features.Commands.RegisterPatient;
using CQRS_Decorator.Application.DTOs;
using CQRS_Decorator.Application.Features.Queries.GetAllDoctors;
using CQRS_Decorator.Application.Features.Queries.GetPatientAppointments;
using CQRS_Decorator.Application.Responses;
using CQRS_Decorator.Decorators;
using CQRS_Decorator.Application.Common.Repositories;
using CQRS_Decorator.Infrastructure.Repositories;
using CQRSDecorate.Net;
using CQRSDecorate.Net.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CQRS_Decorator.Application.Common.Abstractions;
using CQRS_Decorator.Infrastructure.Security;
using CQRS_Decorator.Infrastructure.Auth;

namespace CQRS_Decorator.API.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Healthcare repositories
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            // Healthcare services
            services.AddScoped<IPatientJwtTokenGenerator, PatientJwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Register CQRS with specific handler (will scan assembly)
            services.RegisterCqrsDecorator(typeof(RegisterPatientCommandHandler));

            services.AddValidatorsFromAssemblyContaining<RegisterPatientCommandValidator>();

            // Patient Commands Decorators
            services.Decorate<ICommandHandler<RegisterPatientCommand, GeneralResponse<Guid>>, ValidationDecorator<RegisterPatientCommand, GeneralResponse<Guid>>>();
            services.Decorate<ICommandHandler<RegisterPatientCommand, GeneralResponse<Guid>>, LoggingDecorator<RegisterPatientCommand, GeneralResponse<Guid>>>();

            services.Decorate<ICommandHandler<LoginPatientCommand, GeneralResponse<PatientAuthenticationResult>>, ValidationDecorator<LoginPatientCommand, GeneralResponse<PatientAuthenticationResult>>>();
            services.Decorate<ICommandHandler<LoginPatientCommand, GeneralResponse<PatientAuthenticationResult>>, LoggingDecorator<LoginPatientCommand, GeneralResponse<PatientAuthenticationResult>>>(
            );

            // Appointment Commands Decorators
            services.Decorate<ICommandHandler<BookAppointmentCommand, GeneralResponse<Guid>>, ValidationDecorator<BookAppointmentCommand, GeneralResponse<Guid>>>();
            services.Decorate<ICommandHandler<BookAppointmentCommand, GeneralResponse<Guid>>, LoggingDecorator<BookAppointmentCommand, GeneralResponse<Guid>>>(
            );

            services.Decorate<ICommandHandler<CancelAppointmentCommand, GeneralResponse<bool>>, ValidationDecorator<CancelAppointmentCommand, GeneralResponse<bool>>>();
            services.Decorate<ICommandHandler<CancelAppointmentCommand, GeneralResponse<bool>>, LoggingDecorator<CancelAppointmentCommand, GeneralResponse<bool>>>(
            );
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

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
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Secret"])),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"JWT Authentication Failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                        Console.WriteLine($"JWT Token Validated. Claims: {string.Join(", ", claims ?? Array.Empty<string>())}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"JWT Challenge: {context.Error}, {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();
        }

        public static void AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CQRS Decorator API - Healthcare Appointment System",
                    Version = "v1",
                    Description = "API with CQRS, Decorator Pattern, and JWT Authentication for Healthcare Appointments"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
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
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Add custom schema filters
                c.SchemaFilter<CQRS_Decorator.API.Filters.TimeSpanSchemaFilter>();
                c.SchemaFilter<CQRS_Decorator.API.Filters.BookAppointmentRequestSchemaFilter>();

                // Enable XML comments
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }
    }
}
