namespace TApiMongo.Interfaces
{
    public interface IState<TState>
    {
        TState State { get; set; }
    }
}