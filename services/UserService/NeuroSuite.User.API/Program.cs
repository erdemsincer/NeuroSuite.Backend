using Microsoft.EntityFrameworkCore;
using NeuroSuite.User.Infrastructure.Persistence;
using NeuroSuite.User.Domain.Interfaces;
using NeuroSuite.User.Infrastructure.Repositories;
using NeuroSuite.User.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddSingleton<RabbitMQConsumer>();

// Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// RabbitMQ listener baþlat
using (var scope = app.Services.CreateScope())
{
    var consumer = scope.ServiceProvider.GetRequiredService<RabbitMQConsumer>();
    Task.Run(() => consumer.Start());
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.Migrate();
}
app.Run();
