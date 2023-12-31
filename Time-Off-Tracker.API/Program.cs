using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Time_Off_Tracker.API;
using Time_Off_Tracker.Business.Abstract;
using Time_Off_Tracker.Business.Concrete;
using Time_Off_Tracker.DAL.Abstract;
using Time_Off_Tracker.DAL.Concrete;
using Time_Off_Tracker.DAL.Entity;

var builder = WebApplication.CreateBuilder(args);

var str = builder.Configuration.GetConnectionString("ApiDb");
builder.Services.AddDbContext<ApiContext>(
    c => c.UseNpgsql(builder.Configuration.GetConnectionString("ApiDb"))
);

// Add services to the container.
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IPermissionDal, EfPermissionDal>();
builder.Services.AddScoped<IPermissionService, PermissionManager>();

builder.Services.AddCors(options =>
    options.AddPolicy("SpesificOrigins", policy =>
    policy.WithOrigins("http://localhost:19006",
    "https://time-off-tracker-api-4a95404d0134.herokuapp.com") //Kendi hostunuzu yazınız.
    .AllowAnyHeader()
    .AllowAnyMethod()
    )
);

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Time-Off-Tracker.API", Version = "V2.0 (Stable) Backend Ekip-2" });

    // Yetkilendirme seçeneklerini belirtin
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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Time-Off-Tracker.API V2.0 (Stable) Backend Ekip-2");
});

app.UseCors("SpesificOrigins");

app.UseHttpsRedirection();

app.UseMiddleware<AuthMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
