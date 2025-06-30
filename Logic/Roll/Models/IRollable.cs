namespace Logic.Roll.Models;

public interface IRollable
{
    public enum Modifier
    {
        KeepHighest,
        KeepLowest,
        None
    }

    public string InputString { get; }
    public string OutputString { get; }

    public int Result { get; }
    public bool HasResult => Result != -1;

    public Modifier Mod { get; }

    public async Task Roll(Random random)
    {
        switch (Mod)
        {
            case Modifier.KeepHighest:
                await RollKeepHighest(random);
                break;
            case Modifier.KeepLowest:
                await RollKeepLowest(random);
                break;
            case Modifier.None:
            default:
                await RollTotal(random);
                break;
        }
    }

    protected static Modifier FilterModifier(string diceString)
    {
        if (diceString.EndsWith("kh"))
        {
            return Modifier.KeepHighest;
        }
        return diceString.EndsWith("kl") ? Modifier.KeepLowest : Modifier.None;
    }

    protected static string ModifierSuffix(Modifier mod)
    {
        return mod switch
        {
            Modifier.KeepHighest => "kh",
            Modifier.KeepLowest => "kl",
            _ => string.Empty
        };
    }

    Task RollTotal(Random random);
    Task RollKeepHighest(Random random);
    Task RollKeepLowest(Random random);

}