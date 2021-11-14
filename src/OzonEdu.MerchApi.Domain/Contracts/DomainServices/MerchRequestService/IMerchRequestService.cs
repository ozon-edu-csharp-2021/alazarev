using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;

namespace OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService
{
    public interface IMerchRequestService : IDomainService
    {
        Task<IMerchRequest> CreateMerchRequestAsync(Email employeeEmail, MerchType merchType,
            MerchRequestMode merchRequestMode,
            CancellationToken token = default);

        Task<bool> CheckIfMerchAvailableAsync(int employeeId, MerchType merchType, CancellationToken token = default);

        Task<IEnumerable<IMerchRequest>> GetMerchInfoAsync(Email employeeEmail,
            CancellationToken cancellationToken = default);
    }
}