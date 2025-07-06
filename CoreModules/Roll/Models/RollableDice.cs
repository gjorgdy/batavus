namespace CoreModules.Roll.Models;

public class RollableDice : MathComponent, IRollable
{
    protected enum Modifier
    {
        KeepHighest,
        KeepLowest,
        None
    }

    private readonly int _amount;
    private readonly int _sides;
    private readonly Modifier _mod;

    public Random Random { get; init; }

    protected RollableDice(int amount, int sides, Modifier mod, Random random) : base(Type.Value)
    {
        _amount = amount;
        _sides = sides;
        _mod = mod;
        Random = random;
    }

    public static MathComponent FromString(string diceString, Random random)
    {
        // Has 'kh' or 'kl' for keep highest or lowest
        var mod = FilterModifier(diceString);
        // Remove the modifier from the string if it exists
        if (mod != Modifier.None)
        {
            diceString = diceString[..^2];
        }
        // Split the string into amount and sides
        string[] digits = diceString.Split("d");
        bool bAmount = int.TryParse(digits[0], out int amount);
        bool bSides = int.TryParse(digits[1], out int sides);
        // Throw an exception if the format is invalid
        if (!bAmount || !bSides || amount <= 0 || sides <= 0)
        {
            throw new ArgumentException("Invalid dice format. Use format like '2d6', '1d20kh', or '3d10kl'.");
        }
        return new RollableDice(amount, sides, mod, random);
    }

    public async Task<int> Roll()
    {
        switch (_mod)
        {
            case Modifier.KeepHighest:
                return await RollKeepHighest();
            case Modifier.KeepLowest:
                return await RollKeepLowest();
            case Modifier.None:
            default:
                return await RollTotal();
        }
    }

    public Task<int> RollTotal()
    {
        List<int> rolls = [];
        for (int i = 0; i < _amount; i++)
        {
            rolls.Add(Random.Next(1, _sides + 1));
        }

        Result = new ValueModel(rolls.Sum(), $"({string.Join(" + ", rolls)})");
        return Task.FromResult(Result?.Value ?? -1);
    }

    public Task<int> RollKeepHighest()
    {
        List<int> rolls = [];
        for (int i = 0; i < _amount; i++)
        {
            rolls.Add(Random.Next(1, _sides + 1));
        }

        Result = new ValueModel(rolls.Max(), $"({string.Join(", ", rolls)})kh");
        return Task.FromResult(Result?.Value ?? -1);
    }

    public Task<int> RollKeepLowest()
    {
        List<int> rolls = [];
        for (int i = 0; i < _amount; i++)
        {
            rolls.Add(Random.Next(1, _sides + 1));
        }

        Result = new ValueModel(rolls.Min(), $"({string.Join(", ", rolls)})kl");
        return Task.FromResult(Result?.Value ?? -1);
    }

    protected static Modifier FilterModifier(string diceString)
    {
        if (diceString.EndsWith("kh"))
        {
            return Modifier.KeepHighest;
        }
        return diceString.EndsWith("kl") ? Modifier.KeepLowest : Modifier.None;
    }
}