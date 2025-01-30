using BetterReads.Recommendations.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;

namespace BetterReads.Recommendations.Infra.Services;

public class AzureOpenAiService(IChatCompletionService chatCompletionService, ILogger<AzureOpenAiService> logger) : IAiService
{
    public async Task<string> Process(string input)
    {
        logger.LogInformation("Processing given input: {input}", input);
        var chatHistory = new ChatHistory();
        chatHistory.AddUserMessage(input);
        var result = string.Empty;
        await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            result += response;
        }

        logger.LogInformation("Result for given input: {input} is result: {result}", input, result);
        return result;
    }
}