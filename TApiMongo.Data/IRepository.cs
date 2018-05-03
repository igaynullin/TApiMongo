using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TApiMongo.Interfaces;

namespace TApiMongo.Data
{
    public interface IRepository<TEntity, TId> : IQueryable<TEntity>
        where TEntity : IEntity
    {
        //void SavePartial(TEntity entity);

        /// <summary>
        /// Entity adding
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(TEntity entity);

        /// <summary>
        /// entity async adding
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Many entities adding
        /// </summary>
        /// <param name="entities"></param>
        void AddMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// many entities async adding
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Full enity(document) saving
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isUpsert">create if absent</param>
        void Save(TEntity entity, bool isUpsert = false);

        /// <summary>
        /// Full enity(document) async saving
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isUpsert">добавлять сущность, если она отсутствует в ИД</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task SaveAsync(TEntity entity, bool isUpsert = false, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Entity partial properties saving
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertiesToUpdate">properties to update</param>
        void SavePartial(TEntity entity, params Expression<Func<TEntity, object>>[] propertiesToUpdate);

        /// <summary>
        /// Entity partial properties sync saving
        /// properties to update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <param name="propertiesToUpdate"></param>
        /// <returns></returns>
        Task SavePartialAsync(TEntity entity, CancellationToken token = default(CancellationToken), params Expression<Func<TEntity, object>>[] propertiesToUpdate);

        /// <summary>
        /// entity deleting
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// entity async deleting
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// many entites deleting
        /// </summary>
        /// <param name="entities"></param>
        void DeleteMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// many entites async deleting
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));

        Task AddAsync(TEntity entity);

        IEnumerable<TEntity> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken));
    }
}