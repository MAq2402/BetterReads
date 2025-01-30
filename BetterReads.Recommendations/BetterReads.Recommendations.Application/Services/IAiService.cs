namespace BetterReads.Recommendations.Application.Services;

public interface IAiService
{
    Task<string> Process(string input);
}