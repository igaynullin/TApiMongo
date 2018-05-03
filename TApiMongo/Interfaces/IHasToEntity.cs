namespace TApiMongo.Interfaces
{
    public interface IHasToEntity<TEntity, TViewModel>
    {
        TEntity MapEntity(TViewModel viewModel);
    }
}