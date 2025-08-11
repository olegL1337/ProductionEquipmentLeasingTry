using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductionEquipmentLeasing.Application.Interfaces;
using ProductionEquipmentLeasing.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Infrastructure.Services;
public class ContractProcessingWorker : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ContractProcessingWorker> _logger;

    public ContractProcessingWorker(
        ServiceBusClient client,
        IServiceProvider serviceProvider,
        ILogger<ContractProcessingWorker> logger)
    {
        _processor = client.CreateProcessor("1st-queue");
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        await _processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await _processor.StopProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var message = args.Message;
            var contractMessage = JsonSerializer.Deserialize<ContractProcessingMessage>(message.Body.ToString());

            _logger.LogCritical("Contract {ContractId} was created!", contractMessage.ContractId);

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing contract message");
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service Bus error");
        return Task.CompletedTask;
    }
}
