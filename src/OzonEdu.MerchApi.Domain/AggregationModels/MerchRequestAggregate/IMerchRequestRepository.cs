using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public interface IMerchRequestRepository : IRepository<MerchRequest>
    {
        Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsAsync(
            CancellationToken cancellationToken = default);

        Task<MerchRequest> CreateAsync(MerchRequest request, CancellationToken cancellationToken = default);
        Task<MerchRequest> UpdateAsync(MerchRequest request, CancellationToken cancellationToken = default);
    }
}