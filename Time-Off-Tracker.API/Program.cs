using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.Business.Concrete;
using Time_Off_Tracker.DAL.Abstract;
using Time_Off_Tracker.DAL.Concrete;
using Time_Off_Tracker.DAL.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiContext>();

// Add services to the container.
builder.Services.AddScoped<IUserDal,EfUserDal>();
builder.Services.AddScoped<IUserService,UserManager>();

builder.Services.AddScoped<IPermissionDal, EfPermissionDal>();
builder.Services.AddScoped<IPermissionService, PermissionManager>();


builder.Services.AddCors(options =>
    options.AddPolicy("SpesificOrigins", policy => 
    policy.WithOrigins("http://localhost:19006", 
    "https://time-off-tracker-api-4a95404d0134.herokuapp.com") //Kendi hostunuzu yaz�n�z.
    .AllowAnyHeader())
);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Time-Off-Tracker.API", Version = "v1" });

    // Yetkilendirme se�eneklerini belirtin
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("SpesificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
