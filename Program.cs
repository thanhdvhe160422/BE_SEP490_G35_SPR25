using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Planify_BackEnd.Entities;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Events;
using Planify_BackEnd.Repositories.User;
using Planify_BackEnd.Services.Auths;
using Planify_BackEnd.Services.Events;
using Planify_BackEnd.Services.User;
using Planify_BackEnd.Repositories;
using System.Security.Claims;
using System.Text;
using Planify_BackEnd.Services.SubTasks;
using Planify_BackEnd.Services.Tasks;
using Planify_BackEnd.Repositories.Tasks;
using Planify_BackEnd.Services.Groups;
using Planify_BackEnd.Repositories.Groups;
using Planify_BackEnd.Services.JoinProjects;
using Planify_BackEnd.Repositories.JoinGroups;
using Planify_BackEnd.Services.Campus;
using Newtonsoft.Json;
using Planify_BackEnd.Hub;
using Planify_BackEnd.Services.EventRequests;
using Planify_BackEnd.Repositories.SendRequests;
using Planify_BackEnd.Services.Address;
using Planify_BackEnd.Repositories.Address;
using Planify_BackEnd.Repositories.Categories;
using Planify_BackEnd.Services.Categories;
using Planify_BackEnd.Services.Reports;
using Planify_BackEnd.Repositories.Reports;
using Planify_BackEnd.Services.Users;
using Planify_BackEnd.Services.GoogleDrive;
using Planify_BackEnd.Services.JoinGroups;

var builder = WebApplication.CreateBuilder(args);


var config = builder.Configuration;

// Cấu hình kết nối database
builder.Services.AddDbContext<PlanifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

// Add services to the container
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CampusManager", policy => policy.RequireRole("Campus Manager"));
    options.AddPolicy("EventOrganizer", policy => policy.RequireRole("Event Organizer"));
    options.AddPolicy("Implementer", policy => policy.RequireRole("Implementer"));
    options.AddPolicy("Spectator", policy => policy.RequireRole("Spectator"));
});

// Them Service
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventSpectatorService, EventSpectatorService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddScoped<IProfileService,ProfileService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IJoinProjectService, JoinProjectService>();
builder.Services.AddScoped<ICampusService, CampusService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ISendRequestService, SendRequestService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUserservice, UserService>();
builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddScoped<IJoinGroupService, JoinGroupService>();
// Thêm Repository
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IEventSpectatorRepository, EventSpectatorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISubTaskRepository, SubTaskRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IJoinProjectRepository, JoinProjectRepository>();
builder.Services.AddScoped<ICampusRepository, CampusRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ISendRequestRepository, SendRequestRepository>();
builder.Services.AddScoped<IJoinGroupRepository, JoinGroupRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
// Thêm Authorization
builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; // Tránh vòng lặp mà không tạo `$id`
});

builder.Services.AddSignalR();

// Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Planify API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo format: Bearer {your_token}"
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowCredentials()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Planify API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapHub<EventRequestHub>("/eventRequestHub");

app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();