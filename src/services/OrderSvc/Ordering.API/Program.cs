using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderingContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("OrderingConnectionString")));
// Add services to the container.
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection(nameof(RabbitMQConfiguration)));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IRaiseNewOrderOpening, RaiseNewOrderOpening>();
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
