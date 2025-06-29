namespace Core.Interfaces;

public interface ICommand<T>
{
    Task<T> Execute();
}