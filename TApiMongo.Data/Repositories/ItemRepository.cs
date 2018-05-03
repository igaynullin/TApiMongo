using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TApiMongo.Data.Entities;

namespace TApiMongo.Data.Repositories
{
    public class ItemRepository : MongoRepository<Item>, IItemRepository
    {
        public ItemRepository(string connectionString) : base(connectionString)
        {
        }

        public void SavePartial(Item entity)
        {
            var updates = new List<Expression<Func<Item, object>>>();
            var source = this.FirstOrDefault(e => e.ID == entity.ID);
            if (!string.IsNullOrEmpty(entity.Description) && source.Description != entity.Description)
            {
                updates.Add(item => entity.Description);
            }
            if (!string.IsNullOrEmpty(entity.Name) && source.Name != entity.Name)
            {
                updates.Add(item => entity.Name);
            }
            var sourceTags = string.Join(",", source.Tags);
            var entityTags = string.Join(",", entity.Tags);
            if (!string.IsNullOrEmpty(entityTags) && sourceTags != entityTags)
            {
                updates.Add(item => entity.Tags);
            }

            base.SavePartial(entity, updates.ToArray());
        }

        public async Task SavePartialAsync(Item entity)
        {
            await Task.Run(() =>
             {
                 SavePartial(entity);
             });
        }
    }
}