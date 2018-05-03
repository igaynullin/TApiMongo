namespace TApiMongo.Interfaces
{
    public interface IHasID<TId>
    {
        TId ID { get; set; }
    }
}