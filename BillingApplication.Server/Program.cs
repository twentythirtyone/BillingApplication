using BillingApplication;
using BillingApplication.DataLayer.Repositories;
using BillingApplication.Services.Auth;
using BillingApplication.Services.TariffManager;
using BillingApplication.Repositories;
using BillingApplication.Server.Services.Auth.Roles;
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
using BillingApplication.Server.Services.UserManager;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

if (builder.Configuration["db_connection"] == null || builder.Configuration["db_connection"]!.Length == 0) throw new Exception("no_database_connection");
if (builder.Configuration["secret"] == null || builder.Configuration["secret"]!.Length == 0) throw new Exception("no_secret");

var configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
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

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddDbContext<BillingAppDbContext>(options =>
    options.UseNpgsql(configuration["db_connection"]));

builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IEncrypt, Encrypt>();
builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
builder.Services.AddScoped<ITariffRepository, TariffRepository>();
builder.Services.AddScoped<ITariffManager, TariffManager>();
builder.Services.AddScoped<ISubscriberManager, SubscriberManager>();
builder.Services.AddScoped<RoleAuthorizeFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BillingApplication API", Version = "v1" });

    // Определяем схему безопасности
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите 'Bearer' [пробел] и затем ваш токен",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    // Добавляем требования к безопасности
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

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddReact();
builder.Services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseCors("AllowAllOrigins");
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

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();