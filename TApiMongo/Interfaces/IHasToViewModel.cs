namespace TApiMongo.Interfaces
{
    public interface IHasToViewModel<TViewModel, TEntity>
    {
        TViewModel MapViewModel(TEntity entity);
    }
}