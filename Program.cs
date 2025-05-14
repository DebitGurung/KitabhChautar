using System.Text.Json.Serialization;
using kitabhChautari.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using kitabhChautari.Models;
using KitabhChautari.Services;
using System.Text.Json;
using kitabhChautari.Data;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kitabh Chautari API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Name = "Authorization",
        In = ParameterLocation.Header
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

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    else
    {
        Console.WriteLine($"XML documentation file not found at: {xmlPath}");
    }
});

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
    ?? throw new InvalidOperationException("PostgresConnection not configured");
builder.Services.AddDbContext<KitabhChautariDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions.CommandTimeout(60))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 12;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<KitabhChautariDbContext>()
    .AddDefaultTokenProviders();

var jwtSecret = builder.Configuration["JWT_SECRET"] ?? throw new InvalidOperationException("JWT_SECRET not set in configuration");
var key = Encoding.UTF8.GetBytes(jwtSecret);
var expireHours = double.Parse(builder.Configuration["Jwt:ExpireHours"] ?? "8");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "KitabhChautari",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "KitabhChautariUsers",
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .WithOrigins("https://localhost:7092", "http://localhost:5092")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await SeedDatabase(app);

await app.RunAsync();

async Task SeedDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<KitabhChautariDbContext>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting database migration...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migration completed.");

        var roles = new[] { "Admin", "Staff", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                logger.LogInformation("Creating role: {Role}", role);
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@kitab.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            logger.LogInformation("Creating admin user: {Email}", adminEmail);
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin@SecurePass123!";
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                context.Admins.Add(new Admin
                {
                    Name = "System Admin",
                    Email = adminEmail,
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                });
                await context.SaveChangesAsync();
                logger.LogInformation("Admin user created successfully.");
            }
            else
            {
                logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        var staffEmail = "staff@kitab.com";
        var staffPassword = Environment.GetEnvironmentVariable("STAFF_PASSWORD") ?? "TempStaffPass123!";
        if (await userManager.FindByEmailAsync(staffEmail) == null)
        {
            logger.LogInformation("Creating staff user: {Email}", staffEmail);
            var staff = new IdentityUser
            {
                UserName = staffEmail,
                Email = staffEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(staff, staffPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(staff, "Staff");
                await userManager.ResetAccessFailedCountAsync(staff);
                context.Staffs.Add(new Staff
                {
                    FirstName = "Default",
                    LastName = "Staff",
                    Email = staffEmail,
                    ContactNo = "1234567890",
                    Username = staffEmail
                });
                await context.SaveChangesAsync();
                logger.LogInformation("Staff user created successfully.");
            }
            else
            {
                logger.LogError("Failed to create staff user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
    catch (Npgsql.NpgsqlException ex)
    {
        logger.LogError(ex, "Database seeding failed due to Npgsql error: {Message}", ex.Message);
        throw;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database seeding failed: {Message}", ex.Message);
        throw;
    }
}