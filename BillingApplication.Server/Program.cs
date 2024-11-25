using BillingApplication;
using BillingApplication.Services.Auth;
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
using BillingApplication.Server.Services.Manager.BundleManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Middleware;
using BillingApplication.Server.Quartz;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.TopUpsManager;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using NLog.Web;
using NLog;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.DataLayer.Repositories.Implementations;
using BillingApplication.Server.Services.Manager.InternetManager;

internal class Program
{
    private static void Main(string[] args)
    {
        var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("Initialize");
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureLogging(builder);

            builder.Configuration
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            CheckSecretsAndDbConnection(builder);

            var configuration = builder.Configuration;

            AddAuthentication(builder.Services, configuration);
            ConfigureService(builder.Services, configuration);
            AddSwagger(builder.Services);

            var app = builder.Build();

            ConfigureMiddleWare(app);

            app.Run();
            
        }
        catch(Exception exception)
        {
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }
        
    }
    
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        builder.Host.UseNLog();
    }

    private static void CheckSecretsAndDbConnection(WebApplicationBuilder builder)
    {
        if (builder.Configuration["db_connection"] == null
         || builder.Configuration["db_connection"]!.Length == 0)
            throw new Exception("no_database_connection");
        if (builder.Configuration["secret"] == null
         || builder.Configuration["secret"]!.Length == 0)
            throw new Exception("no_secret");
    }

    private static void ConfigureMiddleWare(WebApplication app)
    {
        app.UseMiddleware<JwtBlacklistMiddleware>();
        app.UseDeveloperExceptionPage();
        app.UseCors("AllowSpecificOrigin");
        app.UseReact(config => { });
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();

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

        services.Configure<MailSettings>(
            configuration.GetSection(nameof(MailSettings))
        );

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
        services.AddScoped<ITopUpsRepository, TopUpsRepository>();
        services.AddScoped<ITopUpsManager, TopUpsManager>();
        services.AddScoped<BillingJob>();
        services.AddScoped<UserActionsJob>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IExtrasRepository, ExtrasRepository>();
        services.AddScoped<IExtrasManager, ExtrasManager>();
        services.AddScoped<ICallsRepository, CallsRepository>();
        services.AddScoped<IMessagesRepository, MessagesRepository>();
        services.AddScoped<IInternetRepository, InternetRepository>();
        services.AddScoped<IInternetManager, InternetManager>();

        services.AddTransient<JobFactory>();
        services.AddTransient<IMailService, MailService>();

        services.AddHostedService<DataSchedulerService>();

        services.AddEndpointsApiExplorer();
        
        services.AddMemoryCache();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IBlacklistService, BlacklistService>();
        services.AddSingleton<IEmailChangeService, EmailChangeService>();

        services.AddReact();
        services.AddJsEngineSwitcher(options => 
            options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
            .AddChakraCore();
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
                Description = "Введите 'Bearer' [пробел] для авторизации",
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