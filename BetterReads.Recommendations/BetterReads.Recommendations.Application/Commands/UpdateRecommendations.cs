using System.Text;
using System.Text.Json;
using BetterReads.Recommendations.Application.Models;
using BetterReads.Recommendations.Application.Services;
using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BetterReads.Recommendations.Application.Commands;

public record UpdateRecommendations(Guid UserId) : IRequest;

public class UpdateRecommendationsCommandHandler(
    IAiService aiService,
    IRecommendationsRepository repository,
    IShelvesService shelvesService,
    ILogger<UpdateRecommendationsCommandHandler> logger) : IRequestHandler<UpdateRecommendations>
{
    public async Task Handle(UpdateRecommendations request, CancellationToken cancellationToken)
    {
        var shelves = await shelvesService.GetShelves(request.UserId);

        var inputForAi = new StringBuilder();
        inputForAi.Append(
            "I need book recommendations based on the following data. Books are placed on the virtual shelves. Each shelf has a name.");
        foreach (var shelf in shelves)
        {
            inputForAi.Append(Environment.NewLine);
            inputForAi.Append($"Shelf: {shelf.Name}");
            inputForAi.Append(Environment.NewLine);
            inputForAi.Append(string.Join(", ", shelf.Books.Select(b => $"{b.Name} {b.Author}")));
        }

        inputForAi.Append(
            "Give me recommendations in the JSON format with properties: title, author and ISBN. I want your response to be just JSON. I need to parse that in my application.");
        var result = await aiService.Process(inputForAi.ToString());

        result = result.TrimStart();
        result = result.TrimEnd();
        result = result.Substring(result.IndexOf("[", StringComparison.Ordinal));
        result = result.Substring(0, result.IndexOf("]", StringComparison.Ordinal) + 1);

        logger.LogInformation("Deserializing the {result} returned from the Ai Service", result);
        var newAiRecommendation = JsonSerializer.Deserialize<List<AiRecommendationResponse>>(result) ??
                                  [];

        var recommendation = await repository.Get(request.UserId);
        if (recommendation == null)
        {
            await repository.Add(new Domain.Entities.Recommendations(Guid.NewGuid(), request.UserId,
                newAiRecommendation.Select(x => new Book(x.Title, x.Author, x.Isbn)).ToList()));
        }
        else
        {
            recommendation.UpdateBooks(newAiRecommendation.Select(x => new Book(x.Title, x.Author, x.Isbn)).ToList());
            await repository.Save(recommendation);
        }
    }
}