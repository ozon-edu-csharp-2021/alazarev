using System.Threading.Tasks;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using Grpc.Core;
using MediatR;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Grpc;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using RequestMerchRequestGrpc = OzonEdu.MerchApi.Grpc.RequestMerchRequest;
using RequestMerchResponseGrpc = OzonEdu.MerchApi.Grpc.RequestMerchResponse;
using GetReceivingMerchInfoRequestGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseGprc = OzonEdu.MerchApi.Grpc.GetMerchInfoResponse;
using RequestMerchResponseHttp = OzonEdu.MerchApi.HttpModels.RequestMerchResponse;
using GetReceivingMerchInfoRequestHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoRequest;
using GetReceivingMerchInfoResponseHttp = OzonEdu.MerchApi.HttpModels.GetMerchInfoResponse;

namespace OzonEdu.MerchApi.GrpcServices
{
    public sealed class MerchApiGrpcService : MerchApiGrpc.MerchApiGrpcBase
    {
        private readonly IMerchRequestService _merchService;
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public MerchApiGrpcService(IMerchRequestService merchService, IMapper mapper,
            IMerchRequestRepository merchRequestRepository, IMediator mediator)
        {
            _merchService = merchService;
            _mapper = mapper;
            _merchRequestRepository = merchRequestRepository;
            _mediator = mediator;
        }

        public override async Task<RequestMerchResponseGrpc> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            var mappedRequest = _mapper.Map<CreateMerchRequest>(request);

            var createMerchRequestCommand =
                new CreateMerchRequestCommand(mappedRequest.EmployeeId, mappedRequest.ManagerEmail,
                    mappedRequest.MerchType,
                    MerchRequestMode.ByRequest);

            var response = await _mediator.Send(createMerchRequestCommand, context.CancellationToken);

            return _mapper.Map<RequestMerchResponseGrpc>(response.Status);
        }

        public override async Task<GetReceivingMerchInfoResponseGprc> GetReceivingMerchInfo(
            GetReceivingMerchInfoRequestGprc request,
            ServerCallContext context)
        {
            var result = await _merchRequestRepository.GetAllEmployeeRequestsAsync(
                EmployeeId.Create(request.EmployeeId),
                context.CancellationToken);


            return _mapper.Map<GetReceivingMerchInfoResponseGprc>(result);
        }
    }
}