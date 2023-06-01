using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using PSAPI.AutoMapper;
using PSAPI.Middleware;
using PSAZServiceBus.Services;
using PSData.Context;
using PSInterfaces;
using PSServices;
using PSServices.ServiceOptions;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

// Add options for services
builder.Services.Configure<ProcessServiceOptions>(builder.Configuration.GetSection(ProcessServiceOptions.ProcessService));


// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

// Add services to the container.

// Do we need to enable signalR
//var useSignalR = builder.Configuration.GetSection("SignalR").GetValue<bool>("Enable");
//if (useSignalR)
//{
//    builder.Services.AddSignalR();
//}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
    });

// Used for enums as strings in input
// TODO, check if we can also use this for output
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.Configure<MvcJsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


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
builder.Services.AddSingleton<IMessageBusFactory<ServiceBusSender>, AZServiceBusFactory>();

// Inject the message service
builder.Services.AddSingleton<IMessageService, AZMessageService>();

builder.Services.AddSingleton<INotifierService, SignalRNotifierService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Show verbose error messages
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

//if (useSignalR)
//{
//    //app.MapHub<ProcessMessageHub>("/processMessageHub");
//}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
