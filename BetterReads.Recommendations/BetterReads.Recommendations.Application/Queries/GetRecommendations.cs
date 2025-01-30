using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Domain.ValueObjects;
using MediatR;

namespace BetterReads.Recommendations.Application.Queries;

public record GetRecommendations(Guid UserId) : IRequest<List<Book>>;

public class GetRecommendationsHandler(IRecommendationsRepository repository) : IRequestHandler<GetRecommendations, List<Book>>
{
    public async Task<List<Book>> Handle(GetRecommendations request, CancellationToken cancellationToken)
    {
        return (await repository.Get(request.UserId))?.Books ?? [];
    }
}