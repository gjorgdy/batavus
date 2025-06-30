namespace Logic.Roll.Models;

using Modifier = IRollable.Modifier;

public class RollableDice : IRollable
{
    public readonly int Amount;
    public readonly int Sides;

    public string InputString => $"{Amount}d{Sides}" + IRollable.ModifierSuffix(Mod);
    public string OutputString => $"{Result}";

    public int Result { get; private set; }
    public Modifier Mod { get; init; }

    protected RollableDice(int amount, int sides, Modifier mod)
    {
        Amount = amount;
        Sides = sides;
        Mod = mod;
        Result = -1; // Default value indicating no roll has been made yet
    }

    public static IRollable FromString(string diceString)
    {
        // Has 'kh' or 'kl' for keep highest or lowest
        var mod = IRollable.FilterModifier(diceString);
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
        return new RollableDice(amount, sides, mod);
    }

    public Task RollTotal(Random random)
    {
        int total = 0;
        for (int i = 0; i < Amount; i++)
        {
            total += random.Next(1, Sides + 1);
        }
        Result = total;
        return Task.CompletedTask;
    }

    public Task RollKeepHighest(Random random)
    {
        List<int> rolls = [];
        for (int i = 0; i < Amount; i++)
        {
            rolls.Add(random.Next(1, Sides + 1));
        }
        Result = rolls.Max();
        return Task.CompletedTask;
    }

    public Task RollKeepLowest(Random random)
    {
        List<int> rolls = [];
        for (int i = 0; i < Amount; i++)
        {
            rolls.Add(random.Next(1, Sides + 1));
        }
        Result = rolls.Min();
        return Task.CompletedTask;
    }
}