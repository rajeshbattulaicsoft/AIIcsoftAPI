using System.Linq.Expressions;

namespace AIIcsoftAPI.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// To get all data we will call this function.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// To get all data we will call this function.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// To get data we will call this function.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TEntity?> GetAsync(object id);

        /// <summary>
        /// To get data we will call this function.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
