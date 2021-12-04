using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using OpenTracing;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Extensions;
using OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests;

namespace OzonEdu.MerchApi.Infrastructure.Handlers.Queries
{
    public class
        GetEmployeeMerchRequestsQueryHandler : IRequestHandler<GetEmployeeMerchRequestsQuery,
            GetEmployeeMerchRequestsResult>
    {
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly ITracer _tracer;
        private readonly IValidator<GetEmployeeMerchRequestsQuery> _validator;

        public GetEmployeeMerchRequestsQueryHandler(IMerchRequestRepository merchRequestRepository,
            IValidator<GetEmployeeMerchRequestsQuery> validator, ITracer trace)
        {
            _merchRequestRepository = merchRequestRepository;
            _validator = validator;
            _tracer = trace;
        }

        public async Task<GetEmployeeMerchRequestsResult> Handle(GetEmployeeMerchRequestsQuery request,
            CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan($"{nameof(GetEmployeeMerchRequestsQueryHandler)}.Handle").StartActive();

            var validationResult =
                await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return GetEmployeeMerchRequestsResult.Fail(
                    validationResult.GetAggregateError("Произошла ошибка валидации"));

            var requests =
                await _merchRequestRepository.GetAllEmployeeRequestsAsync(request.EmployeeId,
                    cancellationToken);

            return GetEmployeeMerchRequestsResult.Success(requests, "Записи успешно получены");
        }
    }
}