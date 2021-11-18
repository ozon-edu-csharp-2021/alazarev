using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public interface IMerchRequestRepository : IRepository<MerchRequest>
    {
        Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(int employeeId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsByModeAsync(MerchRequestMode mode,
            CancellationToken cancellationToken = default);

        Task<MerchRequest> CreateAsync(MerchRequest request, CancellationToken cancellationToken = default);
    }
}