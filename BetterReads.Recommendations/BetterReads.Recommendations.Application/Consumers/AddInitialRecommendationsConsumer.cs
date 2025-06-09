using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Domain.ValueObjects;
using BetterReads.Shared.Application.Commands;
using BetterReads.Shared.Application.Events;
using MassTransit;

namespace BetterReads.Recommendations.Application.Consumers;

public class AddInitialRecommendationsConsumer(IRecommendationsRepository repository) : IConsumer<AddInitialRecommendations>
{
    private readonly List<Book> _initialRecommendations = new()
    {
        new("Project Hail Mary", "Andy Weir", "9780593135204")
    };
    
    public async Task Consume(ConsumeContext<AddInitialRecommendations> context)
    {
        await repository.Add(new Domain.Entities.Recommendations(Guid.NewGuid(), context.Message.UserId, _initialRecommendations));
        await context.Publish(new InitialRecommendationsAdded(context.Message.UserId));
    }
}