using Microsoft.EntityFrameworkCore;
using memory_game_backend.Data.Databases;
using memory_game_backend.Repositories.Implementations;
using memory_game_backend.Repositories.Interfaces;
using memory_game_backend.Services.Implementations;
using memory_game_backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS for all requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => {
        if (type.IsNested)
        {
            return $"{type.DeclaringType.Name}_{type.Name}";
        }
        return type.Name;
    });
});

// Add Database Context
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySQL(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Database"),
        new MySqlServerVersion(new Version(8, 0, 39)) // Ìæ»»ÎªÄãµÄ MySQL °æ±¾
    ));

// Dependency Injection of Repositories and Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScoreRepository, ScoreRepository>();
builder.Services.AddScoped<IScoreService, ScoreService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();