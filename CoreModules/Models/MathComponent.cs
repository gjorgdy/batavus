namespace CoreModules.Roll.Models;

public class MathComponent(MathComponent.Type type)
{
    public enum Type
    {
        Value = 0,
        Addition = 1,
        Subtraction = 2,
    }

    public static MathComponent ValueFromString(string input)
    {
        var c = new MathComponent(Type.Value);
        if (int.TryParse(input, out int value))
        {
            c.Result = new ValueModel(value, input);
        }
        else
        {
            throw new ArgumentException($"Invalid value '{input}' for MathComponent.");
        }
        return c;
    }

    public ValueModel? Result { get; protected set; }
    public Type ComponentType { get; init; } = type;

    public override string ToString()
    {
        return ComponentType switch
        {
            Type.Value => Result?.String ?? "?",
            Type.Addition => "+",
            Type.Subtraction => "-",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}