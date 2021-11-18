using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService
{
    public interface IMerchRequestService : IDomainService
    {
        Task<MerchRequest> CreateMerchRequestAsync(Email employeeEmail, Email managerEmail, MerchType merchType,
            MerchRequestMode merchRequestMode,
            CancellationToken token = default);

        Task<bool> CheckIfMerchAvailableAsync(int employeeId, MerchType merchType, CancellationToken token = default);

        Task<IEnumerable<MerchRequest>> GetMerchInfoAsync(Email employeeEmail,
            CancellationToken cancellationToken = default);
    }
}