namespace Logic.Roll.Models;

public class FlippableCoin : RollableDice
{

    private FlippableCoin(int amount, Modifier mod, Random random) : base(amount, 2, mod, random) {}

    public new static MathComponent FromString(string diceString, Random random)
    {
        // Has 'kh' or 'kl' for keep highest or lowest
        var mod = FilterModifier(diceString);
        // Remove the modifier from the string if it exists
        if (mod != Modifier.None)
        {
            diceString = diceString[..^2];
        }
        // Split the string into amount and sides
        string[] digits = diceString.Split("c");
        bool bAmount = int.TryParse(digits[0], out int amount);
        // Throw an exception if the format is invalid
        if (!bAmount || amount <= 0)
        {
            throw new ArgumentException("Invalid coin format. Use format like '2c', '1ckh', or '3ckl'.");
        }
        return new FlippableCoin(amount, mod, random);

    }
}