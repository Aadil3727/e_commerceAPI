using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.BLDepedency;
using FluentValidation.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Api_crud")));
builder.Services.AddControllersWithViews()
       .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EditDTOValidator>());
// Add business layer dependencies
builder.Services.AddBLDependency(builder.Configuration);
var app = builder.Build();

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
