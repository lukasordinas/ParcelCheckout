using Microsoft.EntityFrameworkCore;
using ParcelCheckout.Api.Extensions;
using ParcelCheckout.Application.Checkout;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ParcelCheckout.Data.Configuration.DbContext>(options =>
options.UseSqlite("Data Source=" + Path.Combine(Environment.CurrentDirectory, @"..\ParcelCheckout.Data\Configuration\config.db")));

builder.Services.AddScoped<ICheckout, Checkout>();

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