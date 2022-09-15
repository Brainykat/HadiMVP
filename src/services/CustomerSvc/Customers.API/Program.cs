using Customers.Data;
using Customers.Data.Repositories;
using Customers.Domain.Interfaces;
using Customers.Services.EventBusServices;
using Customers.Services.Interfaces;
using Customers.Services.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CustomerContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Ef_Postgres_Db")));
// Add services to the container.
//Rabbit
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection(nameof(RabbitMQConfiguration)));
builder.Services.AddTransient<ICustomerBusinessRepository, CustomerBusinessRepository>();
builder.Services.AddTransient<ICustomerBusinessService, CustomerBusinessService>();
builder.Services.AddTransient<IRaiseCustomerAccountOpening, RaiseCustomerAccountOpening>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
