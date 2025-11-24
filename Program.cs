using Microsoft.EntityFrameworkCore;
using Subscribersystem_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SubscriberContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SubscriberDb")));

// CORS-policy för annonssystemet
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAdsClient", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7016", 
                "http://localhost:7016" 
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAdsClient");

app.UseAuthorization();

app.MapControllers();

app.Run();

