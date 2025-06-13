using System.Text.Json;
using BetterReads.Shared.Application.Repositories;
using BetterReads.Shared.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetterReads.Shared.Infra.Services;

public class OutboxBackgroundService(
    IOutboxRepository outboxRepository,
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxBackgroundService> logger) : BackgroundService
{
    private const int Interval = 1000;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var outboxUnprocessedEvents = await outboxRepository.GetUnprocessedEvents();

            using var scope = scopeFactory.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IIntegrationEventPublisher>();
            foreach (var outboxModel in outboxUnprocessedEvents)
            {
                logger.LogInformation($"Processing {outboxModel.Id}");
                try
                {
                    var type = Type.GetType(outboxModel.Type);
                    dynamic? @event =
                        JsonSerializer.Deserialize(outboxModel.Event, type!);
                    if (@event is null)
                    {
                        await outboxRepository.MarkAsFailedToDeliver(outboxModel.Id, "Could not parse event");
                    }
                    else
                    {
                        await publisher.Publish(@event);
                        await outboxRepository.MarkAsProcessed(outboxModel.Id);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"Failed to deliver {e.Message}", e);
                    await outboxRepository.MarkAsFailedToDeliver(outboxModel.Id, e.Message);
                }
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}