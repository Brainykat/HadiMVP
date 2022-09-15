using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Data.Mongo.Configurations;
using Ordering.Data.Mongo.Interfaces;
using Ordering.Data.Mongo.Repositories;
using Ordering.Data.Repositories;
using Ordering.Domain.Interfaces;
using Ordering.Services.EBServices;
using Ordering.Services.Interfaces;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
//postgress
builder.Services.AddDbContext<OrderingContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("OrderingConnectionString")));
//Mongo Db
builder.Services.Configure<OrderDatabaseSettings>(
    builder.Configuration.GetSection("OrderMongoDatabase"));
// Add services to the container.
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection(nameof(RabbitMQConfiguration)));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IMongoOrderRepository, MongoOrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IRaiseNewOrderCreation, RaiseNewOrderCreation>();
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
