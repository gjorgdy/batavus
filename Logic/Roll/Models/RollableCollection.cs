namespace Logic.Roll.Models;

public class RollableCollection : IRollable
{
    public RollableCollection(string input)
    {
        InputString = input;
        // Check if the input is valid
        if (input.StartsWith('(') && input.EndsWith(')'))
        {
            input = input[1..^1]; // Remove the parentheses
        } else throw new ArgumentException("Input must be enclosed in parentheses.");
    }

    public string InputString { get; }
    public string OutputString { get; }
    public int Result { get; }
    public string Value => Result.ToString();

    public Task Roll(Random random)
    {
        throw new NotImplementedException();
    }
}