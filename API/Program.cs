using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using ProductionEquipmentLeasing.API.Middleware;
using ProductionEquipmentLeasing.Application.Common;
using ProductionEquipmentLeasing.Application.Interfaces;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using ProductionEquipmentLeasing.Application.Services;
using ProductionEquipmentLeasing.Infrastructure;
using ProductionEquipmentLeasing.Infrastructure.Repositories;
using ProductionEquipmentLeasing.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["KeyVault:VaultUri"]),
    new DefaultAzureCredential());


builder.Services.AddApplicationInsightsTelemetry(cfg => cfg.ConnectionString = builder.Configuration["appInsightsConnectionString"]);

builder.Services.AddDbContext<EquipmentLeasingContext>(options =>
    options.UseAzureSql(builder.Configuration["sqlConnectionString"]));

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddServiceBusClient(builder.Configuration["serviceBusConnectionString"]);
});

builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
builder.Services.AddScoped<IProductionFacilityService, ProductionFacilityService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<IServiceBusSenderService, ServiceBusSenderService>();
builder.Services.AddHostedService<ContractProcessingWorker>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, typeof(MappingProfile).Assembly);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authentication",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { scheme, new List<string>() }
        });
});

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
