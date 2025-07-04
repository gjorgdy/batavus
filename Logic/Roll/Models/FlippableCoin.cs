namespace Logic.Roll.Models;

public class FlippableCoin : RollableDice
{
    public new string InputString => $"${Amount}c" + ModifierSuffix(Mod);

    public string Value => Result.ToString();

    private FlippableCoin(int amount, Modifier mod) : base(amount, 2, mod) {}

    public new static IRollable FromString(string diceString)
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
        return new FlippableCoin(amount, mod);

    }
}