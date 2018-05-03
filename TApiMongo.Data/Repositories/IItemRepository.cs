using System.Threading.Tasks;
using TApiMongo.Data.Entities;

namespace TApiMongo.Data.Repositories
{
    public interface IItemRepository : IRepository<Item, int>
    {
        void SavePartial(Item entity);

        Task SavePartialAsync(Item entity);
    }
}