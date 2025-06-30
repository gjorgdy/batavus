using Core.Interfaces;
using Logic.Roll.Models;

namespace Logic.Roll;

public class RollCommand : ICommand<RollResponse>
{

    private List<IRollable> Rollables { get; }

    public RollCommand(string input)
    {
        string[] components = input.Split('+', '-', '/');
        Rollables = components.Select(component =>
        {
            component = component.Trim();

            if (component.Contains('d'))
            {
                return RollableDice.FromString(component);
            }

            return component.Contains('c') ? FlippableCoin.FromString(component) : null;
        })
        .Where(rollable => rollable != null)
        .Select(rollable => rollable!)
        .ToList();
    }

    public Task<RollResponse> Execute()
    {
        var random = new Random();
        int total = Rollables.Select(async (r) =>
        {
            await r.Roll(random);
            return r.Result;
        })
        .Sum(r => r.Result);

        return Task.FromResult(new RollResponse(
            Components: Rollables.Select(r => r.OutputString).ToArray(), // Placeholder for actual components));
            Total: total // Placeholder for actual total
        ));
    }
}