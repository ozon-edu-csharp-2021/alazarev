using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Infrastructure.Common;

namespace OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest
{
    public class CreateMerchRequestResult : IResult
    {
        public bool IsSuccess { get; private init; }
        public string Message { get; private init; }
        public MerchRequestStatus Status { get; private init; }
        public int RequestId { get; private init; }

        public static CreateMerchRequestResult Fail(string errorMessage)
            => new()
            {
                IsSuccess = false,
                Message = errorMessage,
            };

        public static CreateMerchRequestResult Success(MerchRequestStatus status, int requestId, string message)
            => new()
            {
                IsSuccess = true,
                Message = message,
                Status = status,
                RequestId = requestId
            };
    }
}