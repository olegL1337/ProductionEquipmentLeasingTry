using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using ProductionEquipmentLeasing.Application.Interfaces.Services;
using ProductionEquipmentLeasing.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Infrastructure.Services;
public class ServiceBusSenderService : IServiceBusSenderService
{
    private readonly ServiceBusSender _sender;
    private readonly ILogger<ServiceBusSenderService> _logger;

    public ServiceBusSenderService(
        ServiceBusClient client,
        ILogger<ServiceBusSenderService> logger)
    {
        _sender = client.CreateSender("1st-queue");
        _logger = logger;
    }

    public async Task SendContractForProcessingAsync(Guid contractId)
    {
        var message = new ContractProcessingMessage(
            contractId,
            DateTimeOffset.UtcNow);

        await _sender.SendMessageAsync(
            new ServiceBusMessage(JsonSerializer.Serialize(message)));

        _logger.LogCritical("Sent contract {ContractId} for processing", contractId);
    }
}
