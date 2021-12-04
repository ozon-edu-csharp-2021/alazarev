using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Queries.GetEmployeeMerchRequests;
using OzonEdu.MerchApi.Infrastructure.StockApi;

namespace OzonEdu.MerchApi.Controllers.V1
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class MerchController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;

        public MerchController(IMediator mediator, StockApiGrpc.StockApiGrpcClient stockApiGrpcClient, ITracer tracer)
        {
            _mediator = mediator;
            _tracer = tracer;
        }

        [HttpPost("create")]
        public async Task<ActionResult<RequestMerchResponse>> RequestMerch(
            [FromBody] CreateMerchRequest request,
            CancellationToken token)
        {
            var createMerchRequestCommand =
                new CreateMerchRequestCommand(request.EmployeeId, request.ManagerEmail,
                    request.MerchType,
                    MerchRequestMode.ByRequest);
            var response = await _mediator.Send(createMerchRequestCommand, token);
            return Ok(response);
        }

        [HttpGet("check/{employeeId:int}")]
        public async Task<ActionResult<GetMerchInfoResponse>> GetMerchInfo(
            int employeeId, CancellationToken token)
        {
            var query = new GetEmployeeMerchRequestsQuery(employeeId);
            var response = await _mediator.Send(query, token);
            return Ok(response);
        }
    }
}