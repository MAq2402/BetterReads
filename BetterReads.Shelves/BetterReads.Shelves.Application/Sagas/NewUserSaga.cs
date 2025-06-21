using BetterReads.Shared.Application.Commands;
using BetterReads.Shared.Application.Events;
using MassTransit;

namespace BetterReads.Shelves.Application.Sagas;

public class NewUserInstance : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string? CurrentState { get; set; }
    public int Version { get; set; }
}

public class NewUserSaga : MassTransitStateMachine<NewUserInstance>
{
    public State CreatingDefaultShelves { get; set; }
    public State AddingInitialRecommendations { get; set; }
    public Event<UserRegistered> UserRegistered { get; set; }
    public Event<DefaultShelvesCreated> DefaultShelvesCreated { get; set; }
    public Event<InitialRecommendationsAdded> InitialRecommendationsAdded { get; set; }

    public NewUserSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserRegistered, e => e.CorrelateById(m => m.Message.Id));
        Event(() => DefaultShelvesCreated, e => e.CorrelateById(m => m.Message.UserId));
        Event(() => InitialRecommendationsAdded, e => e.CorrelateById(m => m.Message.UserId));

        Initially(
            When(UserRegistered)
                .TransitionTo(CreatingDefaultShelves)
                .Publish(context => new CreateDefaultShelves(context.Message.Id)));

        During(CreatingDefaultShelves,
            When(DefaultShelvesCreated)
                .TransitionTo(AddingInitialRecommendations)
                .Publish(context => new AddInitialRecommendations(context.Message.UserId)));

        During(AddingInitialRecommendations,
            When(InitialRecommendationsAdded)
                .Finalize());
    }
}