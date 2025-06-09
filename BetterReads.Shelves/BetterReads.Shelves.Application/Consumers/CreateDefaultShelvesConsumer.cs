using BetterReads.Shared.Application.Commands;
using BetterReads.Shared.Application.Events;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;
using MassTransit;

namespace BetterReads.Shelves.Application.Consumers;

public class CreateDefaultShelvesConsumer(IShelvesRepository repository) : IConsumer<CreateDefaultShelves>
{
    public async Task Consume(ConsumeContext<CreateDefaultShelves> context)
    {
        var userId = context.Message.UserId;
        
        await repository.AddShelf(Shelf.CurrentlyReading(userId));
        await repository.AddShelf(Shelf.Read(userId));
        await repository.AddShelf(Shelf.WantToRead(userId));
        
        await context.Publish(new DefaultShelvesCreated(context.Message.UserId));
    }
}