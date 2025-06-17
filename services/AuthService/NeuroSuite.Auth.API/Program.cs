using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeuroSuite.Auth.Application.Features.Auth.Commands.Register;
using NeuroSuite.Auth.Domain.Interfaces;
using NeuroSuite.Auth.Infrastructure.Persistence;
using NeuroSuite.Auth.Infrastructure.Repositories;
using NeuroSuite.Auth.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL ba�lant�s�
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
});


// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommandHandler).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Scoped ba��ml�l�klar
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// JWT ayarlar�n� y�kle
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Auth & Swagger
builder.Services.AddAuthentication().AddJwtBearer(); // daha sonra config yap�lacak
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
