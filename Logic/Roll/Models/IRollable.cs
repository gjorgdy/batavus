namespace Logic.Roll.Models;

public interface IRollable
{

    protected Random Random { get; init; }

    public Task<int> Roll();

}