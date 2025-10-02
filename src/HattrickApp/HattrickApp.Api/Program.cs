using Carter;
using FluentValidation;
using HattrickApp.Api.Common.ConfigurationOptions;
using HattrickApp.Api.Persistence;
using HattrickApp.Api.Seeder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<HattrickAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.Configure<MediatrOptions>(builder.Configuration.GetSection(nameof(MediatrOptions)));

builder.Services.AddMediatR(config =>
{
    MediatrOptions? mediatrOptions = builder.Configuration.GetSection(nameof(MediatrOptions)).Get<MediatrOptions>();
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.LicenseKey = mediatrOptions?.CommunityLicenseKey;
});

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

app.MapCarter();
app.UseHttpsRedirection();

app.Run();
