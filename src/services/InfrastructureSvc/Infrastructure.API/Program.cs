using Infrastructure.API.Configurations;
using Infrastructure.API.HostedServices;
using Infrastructure.API.Interfaces;
using Infrastructure.API.Services;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection(nameof(RabbitMQConfiguration)));
builder.Services.Configure<GovernmentAPIConfigs>(builder.Configuration.GetSection(nameof(GovernmentAPIConfigs)));
builder.Services.AddHttpClient();
builder.Services.AddTransient<IGovTaxLoggingService, GovTaxLoggingService>();
// Add services to the container.
builder.Services.AddHostedService<NewOrderEBConsumer>();
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
