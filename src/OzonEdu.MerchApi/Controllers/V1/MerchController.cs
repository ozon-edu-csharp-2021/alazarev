using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public MerchController(IMediator mediator, StockApiGrpc.StockApiGrpcClient stockApiGrpcClient)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<ActionResult<RequestMerchResponse>> RequestMerch(
            [FromBody] CreateMerchRequest request,
            CancellationToken token)
        {
            var createMerchRequestCommand =
                new CreateMerchRequestCommand(request.EmployeeEmail, request.ManagerEmail, request.ClothingSize,
                    request.MerchType,
                    MerchRequestMode.ByRequest);
            var response = await _mediator.Send(createMerchRequestCommand, token);
            return Ok(response);
        }

        [HttpGet("check/{employeeEmail}")]
        public async Task<ActionResult<GetMerchInfoResponse>> GetMerchInfo(
            string employeeEmail, CancellationToken token)
        {
            var query = new GetEmployeeMerchRequestsQuery(employeeEmail);
            var response = await _mediator.Send(query, token);
            return Ok(response);
        }
    }
}