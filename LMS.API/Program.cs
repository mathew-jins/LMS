using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Application.Services;
using LMS.Infrastructure.DbContext;
using LMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// PostgreSQL Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeavePeriodService, LeavePeriodService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT Auth
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// JWT Authentication Configuration
var jwtKeyStr = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKeyStr)) jwtKeyStr = "LMS_Super_Secret_Key_For_Jwt_Auth_12345!@#";
var keyBytes = Encoding.ASCII.GetBytes(jwtKeyStr);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // context.Database.Migrate();
    if (!context.Users.Any())
    {
        context.Users.Add(new LMS.Domain.Entities.User 
        { 
            FirstName = "Admin", 
            LastName = "User", 
            Email = "admin@lms.com", 
            Role = LMS.Domain.Enums.Role.Admin, 
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        });
        context.LeaveTypes.AddRange(
            new LMS.Domain.Entities.LeaveType { LeaveTypeName = "Casual Leave", MaxDaysPerYear = 12, Description = "Casual Leave" },
            new LMS.Domain.Entities.LeaveType { LeaveTypeName = "Sick Leave", MaxDaysPerYear = 12, Description = "Sick Leave" },
            new LMS.Domain.Entities.LeaveType { LeaveTypeName = "Earned Leave", MaxDaysPerYear = 12, Description = "Earned Leave" }
        );
        context.SaveChanges();
    }
    
    // Enforcement of default 12 leaves for existing DB data
    var existingLeaveTypes = context.LeaveTypes.Where(lt => lt.MaxDaysPerYear != 12).ToList();
    if(existingLeaveTypes.Any())
    {
        foreach(var lt in existingLeaveTypes)
        {
            lt.MaxDaysPerYear = 12;
        }
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
