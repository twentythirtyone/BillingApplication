using BillingApplication;
using BillingApplication.DataLayer.Repositories;
using BillingApplication.Services.Auth;
using BillingApplication.Repositories;
using BillingApplication.Services.Auth.Roles;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using React.AspNet;
using System.Security.Claims;
using System.Text;
using BillingApplication.Server.DataLayer.Repositories;
using BillingApplication.Server.Services.Manager.BundleManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Middleware;
using BillingApplication.Server.Quartz;
using Microsoft.Extensions.Hosting;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Manager.PaymentsManager;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

        if (builder.Configuration["db_connection"] == null 
         || builder.Configuration["db_connection"]!.Length == 0) 
            throw new Exception("no_database_connection");
        if (builder.Configuration["secret"] == null 
         || builder.Configuration["secret"]!.Length == 0) 
            throw new Exception("no_secret");

        var configuration = builder.Configuration;

        AddAuthentication(builder.Services, configuration);
        ConfigureService(builder.Services, configuration);
        AddSwagger(builder.Services);

        var app = builder.Build();
        app.UseMiddleware<JwtBlacklistMiddleware>();
        app.UseDeveloperExceptionPage();
        app.UseCors("AllowSpecificOrigin");
        app.UseReact(config => { });
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();

        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                DataScheduler.Start(serviceProvider);
            }
            catch (Exception)
            {
                throw;
            }
        }

        app.MapControllers();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }

    private static void ConfigureService(IServiceCollection services, ConfigurationManager configuration)
    {
        
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        services.AddDbContext<BillingAppDbContext>(options =>
            options.UseNpgsql(configuration["db_connection"]));

        services.AddScoped<IAuth, Auth>();
        services.AddScoped<Auth>();
        services.AddScoped<IEncrypt, Encrypt>();
        services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        services.AddScoped<ITariffRepository, TariffRepository>();
        services.AddScoped<IBundleRepository, BundleRepository>();
        services.AddScoped<ITariffManager, TariffManager>();
        services.AddScoped<ISubscriberManager, SubscriberManager>();
        services.AddScoped<IMessagesManager, MessagesManager>();
        services.AddScoped<ICallsManager, CallsManager>();
        services.AddScoped<IBundleManager, BundleManager>();
        services.AddScoped<RoleAuthorizeFilter>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentsManager, PaymentsManager>();
        services.AddScoped<DataJob>();
        services.AddScoped<IEmailSender, EmailSender>();

        services.AddTransient<JobFactory>();


        services.AddEndpointsApiExplorer();
        
        services.AddMemoryCache();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IBlacklistService, BlacklistService>();

        services.AddReact();
        services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();
    }

    private static void AddAuthentication(IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddAuthentication(options =>
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
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["secret"]!)),
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BillingApplication API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "������� 'Bearer' [������] � ����� ��� �����",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
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
                    new string[] {}
                }
            });
        });
    }
}