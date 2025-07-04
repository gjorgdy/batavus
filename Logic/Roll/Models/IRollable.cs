using Logic.Models;

namespace Logic.Roll.Models;

public interface IRollable : IMathComponent
{
    public new string Value => Result.ToString();

    public string InputString { get; }
    public string OutputString { get; }

    public int Result { get; }
    public bool HasResult => Result != -1;

    public Task Roll(Random random);

}