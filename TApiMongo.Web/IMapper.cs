namespace TApiMongo.Web
{
    internal interface IMapper<TEntity, TViewModel>
    {
        TEntity MapEntity(TViewModel viewModel);
        TViewModel MapViewModel(TEntity entity);
    }
}