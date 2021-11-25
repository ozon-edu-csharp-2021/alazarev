using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Common;

namespace OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests
{
    public class GetEmployeeMerchRequestsResult : IResult
    {
        public bool IsSuccess { get; private init; }
        public string Message { get; private init; }

        public IEnumerable<GetEmployeeMerchRequest> Requests { get; private init; }

        public static GetEmployeeMerchRequestsResult Fail(string errorMessage)
            => new()
            {
                IsSuccess = false,
                Message = errorMessage,
            };

        public static GetEmployeeMerchRequestsResult Success(IEnumerable<MerchRequest> requests, string message)
            => new()
            {
                IsSuccess = true,
                Message = message,
                Requests = requests.Select(r => new GetEmployeeMerchRequest()
                {
                    Status = r.Status.Name,
                    RequestId = r.Id,
                    MerchType = r.RequestedMerchType,
                    StartedAt = r.StartedAt,
                    ReservedAt = r.ReservedAt
                })
            };
    }
}