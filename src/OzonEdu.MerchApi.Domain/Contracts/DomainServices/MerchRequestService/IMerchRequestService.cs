using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService
{
    public interface IMerchRequestService : IDomainService
    {
        Task<bool> CheckIfMerchAvailableAsync(EmployeeEmail employeeEmail, MerchType merchType,
            CancellationToken token = default);
    }
}