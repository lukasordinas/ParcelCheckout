using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParcelCheckout.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ParcelCheckout.Api.Data.Configuration.DbContext>(options =>
options.UseSqlite("Data Source=" + Path.Combine(Environment.CurrentDirectory, @"Data\Configuration\config.db")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();