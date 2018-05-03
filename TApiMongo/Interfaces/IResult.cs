namespace TApiMongo.Interfaces
{
    public interface IResult<T>
    {
        T Result { get; set; }
    }
}