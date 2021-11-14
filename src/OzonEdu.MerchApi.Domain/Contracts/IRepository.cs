using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.Contracts
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    /// <typeparam name="TAggregationRoot">Объект сущности для управления</typeparam>
    public interface IRepository<TAggregationRoot> where TAggregationRoot : IEntity

    {
        /// <summary>
        /// Объект <see cref="IUnitOfWork"/>
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Создать новую сущность
        /// </summary>
        /// <param name="itemToCreate">Объект для создания</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Созданная сущность</returns>
        Task<TAggregationRoot> CreateAsync(TAggregationRoot itemToCreate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновить существующую сущность
        /// </summary>
        /// <param name="itemToUpdate">Объект для создания</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Обновленная сущность сущность</returns>
        Task<TAggregationRoot> UpdateAsync(TAggregationRoot itemToUpdate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить запись по идентфиикатору
        /// </summary>
        /// <param name="id">Идентфиикатор</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TKey">Тип идентфиикатора</typeparam>
        /// <returns>Запись</returns>
        Task<TAggregationRoot> GetAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TKey">Тип идентфиикатора</typeparam>
        /// <returns></returns>
        Task DeleteAsync(TAggregationRoot entity, CancellationToken cancellationToken = default);
    }
}