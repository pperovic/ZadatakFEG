using HattrickApp.Api.Persistence;
using HattrickApp.Api.Seeder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HattrickAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Run the seeder
using (IServiceScope scope = app.Services.CreateScope())
{
    HattrickAppDbContext dbContext = scope.ServiceProvider.GetRequiredService<HattrickAppDbContext>();
    await DbSeeder.SeedAsync(dbContext);
}

app.UseHttpsRedirection();

app.Run();
