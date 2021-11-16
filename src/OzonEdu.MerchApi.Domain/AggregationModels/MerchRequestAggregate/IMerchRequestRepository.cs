using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public interface IMerchRequestRepository : IRepository<IMerchRequest>
    {
        Task<IEnumerable<IMerchRequest>> GetAllEmployeeRequestsAsync(int employeeId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<IMerchRequest>> GetAllWaitingForSupplyRequestsByModeAsync(MerchRequestMode mode,
            CancellationToken cancellationToken = default);
    }
}