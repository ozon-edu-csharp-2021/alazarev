using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeMerchRequestRepository : FakeRepository<MerchRequest>, IMerchRequestRepository
    {
        public Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(int employeeId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.Where(r => r.EmployeeId.Value == employeeId));
        }

        public Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsByModeAsync(MerchRequestMode mode,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.Where(r =>
                r.Status.Equals(MerchRequestStatus.WaitingForSupply) && r.Mode.Equals(mode)));
        }

        public FakeMerchRequestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}