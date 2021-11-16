using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate
{
    /// <summary>
    /// HR менеджер.
    ///
    /// Изначально думал седалть через роли сотрудника, но решил, что лучше выделить отдельный агрегат,
    /// т.к. у него могут быть, как уникальные поля, так и уникальные логика
    ///  
    /// </summary>
    public sealed class HumanResourceManager : Entity
    {
        /// <summary>
        /// HR это в первую очередь сотрудник
        /// </summary>
        public EmployeeId EmployeeId { get; private set; }

        public HumanResourceManager(EmployeeId employeeId)
        {
            EmployeeId = employeeId;
        }

        public HumanResourceManager(int id, EmployeeId employeeId)
        {
            EmployeeId = employeeId;
            Id = id;
        }
    }
}