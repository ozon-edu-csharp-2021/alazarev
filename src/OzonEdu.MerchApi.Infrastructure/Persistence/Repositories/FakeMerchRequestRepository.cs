using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Repositories;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class FakeMerchRequestRepository : FakeRepository<MerchRequest>, IMerchRequestRepository
    {
        public FakeMerchRequestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(EmployeeEmail employeeEmail,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Enumerable.Empty<MerchRequest>());
        }

        public Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsByModeAsync(MerchRequestMode mode,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Enumerable.Empty<MerchRequest>());
        }

        public Task<MerchRequest> CreateAsync(MerchRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(request);
        }
    }
}