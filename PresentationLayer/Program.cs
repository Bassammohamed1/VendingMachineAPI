using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataLayer.Data;
using DataLayer.Data.Seeds;
using DataLayer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Extentions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(typeof(IProductsService), typeof(ProductsService));
builder.Services.AddCustomJwtAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
var usermanager = services.GetRequiredService<UserManager<User>>();
await Roles.SeedSeller(rolemanager);
await Roles.SeedBuyer(rolemanager);
await Roles.SeedAdmin(rolemanager);
await Users.SeedAdmin(usermanager);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
