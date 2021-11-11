using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService
{
    public interface IMerchRequestService : IDomainService
    {
        Task<MerchRequest> CreateMerchRequestAsync(string employeeEmail, MerchType merchType,
            MerchRequestMode merchRequestMode,
            CancellationToken token = default);

        Task<bool> CheckIfMerchAvailableAsync(int employeeId, MerchType merchType, CancellationToken token = default);

        Task ReserveStockAsync(MerchRequest request, CancellationToken cancellationToken = default);

        Task CheckStockAsync(MerchRequest request, CancellationToken cancellationToken = default);

        Task<IEnumerable<MerchRequest>> GetMerchInfoAsync(string employeeEmail,
            CancellationToken cancellationToken = default);
    }
}