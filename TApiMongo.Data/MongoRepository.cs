using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using TApiMongo.Data.JsonConverters;
using TApiMongo.Interfaces;

namespace TApiMongo.Data
{
    public abstract class MongoRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : IEntity, IHasID<int>
    {
        private readonly string _collectionName;

        protected MongoRepository(IMongoClient client, string dbName)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrEmpty(dbName))
                throw new ArgumentNullException(nameof(dbName));
            Client = client;
            EntityType = typeof(TEntity);
            DataBase = Client.GetDatabase(dbName);

            var attr = Attribute.GetCustomAttribute(EntityType, typeof(MongoCollectionName)) as MongoCollectionName;
            if (attr != null && !string.IsNullOrEmpty(attr.Name))
                _collectionName = attr.Name;
            else
                _collectionName = EntityType.Name;
        }

        protected MongoRepository(string connectionString) :
            this(new MongoClient(new MongoUrl(connectionString)), new MongoUrl(connectionString).DatabaseName)
        {
        }

        private Type EntityType { get; }
        private IMongoClient Client { get; }

        private IMongoDatabase DataBase { get; }

        private IMongoCollection<TEntity> Collection => DataBase.GetCollection<TEntity>(_collectionName);

        #region IQuerable

        public Type ElementType => Collection.AsQueryable().ElementType;

        public Expression Expression => Collection.AsQueryable().Expression;

        public IQueryProvider Provider => Collection.AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.AsQueryable().GetEnumerator();
        }

        #endregion IQuerable

        protected virtual void PreAddItem(TEntity entity)
        {
        }

        public void Save(TEntity entity, bool isUpsert = false)
        {
            if (isUpsert)
                PreAddItem(entity);
            Collection.ReplaceOne(x => x.ID == entity.ID, entity, new UpdateOptions { IsUpsert = isUpsert });
        }

        public Task SaveAsync(TEntity entity, bool isUpsert = false, CancellationToken token = default(CancellationToken))
        {
            if (isUpsert)
                PreAddItem(entity);
            return Collection.ReplaceOneAsync(x => x.ID == entity.ID, entity, new UpdateOptions { IsUpsert = isUpsert }, token);
        }

        public void SavePartial(TEntity entity, params Expression<Func<TEntity, object>>[] updates)
        {
            var updateDefinition = Builders<BsonDocument>.Update.Combine(ConvertUpdateDefinition(entity, updates));
            DataBase.GetCollection<BsonDocument>(_collectionName).UpdateOne(x => x[nameof(entity.ID)] == entity.ID, updateDefinition);
        }

        public Task SavePartialAsync(TEntity entity, CancellationToken token = default(CancellationToken), params Expression<Func<TEntity, object>>[] updates)
        {
            var updateDefinition = Builders<BsonDocument>.Update.Combine(ConvertUpdateDefinition(entity, updates));
            return DataBase.GetCollection<BsonDocument>(_collectionName).UpdateOneAsync(x => x[nameof(entity.ID)] == entity.ID, updateDefinition, null, token);
        }

        private IEnumerable<UpdateDefinition<BsonDocument>> ConvertUpdateDefinition(TEntity entity, params Expression<Func<TEntity, object>>[] updates)
        {
            var map = (BsonClassMap<TEntity>)BsonClassMap.LookupClassMap(typeof(TEntity));
            return (from update in updates
                    let fieldMap = map.GetMemberMap(update)
                    let exp = update.Compile()
                    let value = exp(entity)
                    let sz = JsonConvert.SerializeObject(value, new JsonSerializerSettings
                    {
                        ContractResolver = new BsonDataContractResolver(),
                        Converters = new List<JsonConverter> { new LongJsonConverter(), new DateTimeJsonConverter() }
                    })
                    select $"{{$set : {{{fieldMap.ElementName} : {sz}}}}}").Select(updateStr => (UpdateDefinition<BsonDocument>)updateStr);
        }

        public void Add(TEntity entity)
        {
            PreAddItem(entity);
            Collection.InsertOne(entity);
        }

        public Task AddAsync(TEntity entity)
        {
            return Task.Run(() => Add(entity));
        }

        public Task AddAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            PreAddItem(entity);
            return Collection.InsertOneAsync(entity, new InsertOneOptions { BypassDocumentValidation = false }, token);
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            Collection.InsertMany(entities);
        }

        public Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken))
        {
            return Collection.InsertManyAsync(entities, null, token);
        }

        public void Delete(TEntity entity)
        {
            Collection.DeleteOne(x => x.ID == entity.ID);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.Run(() => { Delete(entity); });
        }

        public Task DeleteAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            return Collection.DeleteOneAsync(x => x.ID == entity.ID, token);
        }

        public void DeleteMany(IEnumerable<TEntity> entities)
        {
            var listIds = entities.Select(x => x.ID).ToList();
            Collection.DeleteMany(x => listIds.Contains(x.ID));
        }

        public Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken))
        {
            var listIds = entities.Select(x => x.ID).ToList();
            return Collection.DeleteManyAsync(x => listIds.Contains(x.ID), token);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Collection.AsQueryable().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.FromResult(GetAll());
        }

        //public virtual void SavePartial(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public virtual Task SavePartialAsync(TEntity entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}