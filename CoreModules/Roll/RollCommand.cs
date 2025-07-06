using Core.Interfaces;
using CoreModules.Roll.Models;

namespace CoreModules.Roll;

public class RollCommand : ICommand<RollResponse>
{

    private List<MathComponent> Components { get; set; }

    public RollCommand(string input)
    {
        string[] components = input.Split(' ');
        var random = new Random();
        Components = components.Select(component =>
        {
            return component.Trim() switch
            {
                { } s when s.All(char.IsDigit) => MathComponent.ValueFromString(s),
                { } s when s.Contains('d') => RollableDice.FromString(s, random),
                { } s when s.Contains('c') => FlippableCoin.FromString(s, random),
                "+" => new MathComponent(MathComponent.Type.Addition),
                "-" => new MathComponent(MathComponent.Type.Subtraction),
                _ => throw new ArgumentException(
                    $"Component '{component}' is not a valid rollable or mathematical component.")
            };
        })
        .ToList();
    }

    public async Task<RollResponse> Execute()
    {
        int total = 0;
        List<string> resultStrings = [];
        List<string> calculationStrings = [];
        bool add = true;
        foreach (var t in Components)
        {
            switch (t.ComponentType)
            {
                case MathComponent.Type.Addition:
                    add = true;
                    resultStrings.Add("+");
                    break;
                case MathComponent.Type.Subtraction:
                    add = false;
                    resultStrings.Add("-");
                    break;
                case MathComponent.Type.Value:
                    // Get the value from the component
                    int res = t is IRollable rollable
                        ? await rollable.Roll()
                        : t.Result?.Value ?? 0;
                    // If the rollable has a result, use it
                    if (add) total += res;
                    else total -= res;
                    resultStrings.Add(res.ToString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            calculationStrings.Add(t.ToString());
        }

        return new RollResponse(
            ResultStrings: resultStrings.ToArray(),
            CalculationStrings: calculationStrings.ToArray(), // Placeholder for actual components));
            Total: total // Placeholder for actual total
        );
    }

}