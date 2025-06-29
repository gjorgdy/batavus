using Core.Interfaces;

namespace Logic.Roll;

public class RollCommand(string input) : ICommand<RollResponse>
{
    public Task<RollResponse> Execute()
    {
        return Task.FromResult(new RollResponse(
            Components: input.Split(" "), // Placeholder for actual components));
            Total: 0 // Placeholder for actual total
        ));
    }
}