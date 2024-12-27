using Delivery_BLL.Configs;
using Delivery_BLL.Services.IServices;
using Delivery_BLL.Services;
using Delivery_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();