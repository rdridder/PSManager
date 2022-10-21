using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using PSAPI.AutoMapper;
using PSAPI.Middleware;
using PSAZServiceBus.Services;
using PSData.Context;
using PSInterfaces;
using PSServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProcessContext>(opt =>
// Uncomment to use in memory database
//    opt.UseInMemoryDatabase("PSManager"));

// Comment if you want to use in memory database
    opt.UseSqlServer(builder.Configuration.GetConnectionString("PSManager"), a => a.MigrationsAssembly("PSAPI"))
);

// Only for development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IProcessService, ProcessService>();

// Add a service bus client
builder.Services.AddAzureClients(clientFactoryBuilder =>
{
    clientFactoryBuilder.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
});
// Add a factory to create the senders
builder.Services.AddSingleton<IMessageBusFactory<ServiceBusSender>, AzureServiceBusFactory>();

// Inject the message service
builder.Services.AddSingleton<IMessageService, MessageService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
