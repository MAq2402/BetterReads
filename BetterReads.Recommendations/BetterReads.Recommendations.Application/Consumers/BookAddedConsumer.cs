using BetterReads.Recommendations.Application.Commands;
using BetterReads.Shared.Application.Events;
using MassTransit;
using MediatR;

namespace BetterReads.Recommendations.Application.Consumers;

public class BookAddedConsumer(IMediator mediator) : IConsumer<BookAdded>
{
    public async Task Consume(ConsumeContext<BookAdded> context)
    {
        await mediator.Send(new UpdateRecommendations(context.Message.UserId));
    }
}