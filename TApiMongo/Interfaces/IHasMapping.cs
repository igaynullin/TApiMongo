namespace TApiMongo.Interfaces
{
    public interface IHasMapping<out TResult, in TModel>
    {
        TResult Map(TModel model);
    }
}