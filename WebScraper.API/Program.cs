using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebScraper.API.Identity.Manager;
using WebScraper.Database;
using WebScraper.Database.Models;
using WebScrapper.Core;
using WebScrapper.Core.Services;
using WebWorkPlace.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<WebScrapperDbContext>()
    // .AddUserManager<UserManager>()
    .AddSignInManager()
    .AddApiEndpoints();
builder.Services.AddDbContext<WebScrapperDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.Register();
builder.Services.AddHostedService<CapitolScraperHostedService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();
app.UseHttpsRedirection();


app.Run();
