using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Infrastructure.StockApi;

namespace OzonEdu.MerchApi.Controllers.V1
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class MerchPackController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMerchPackRepository _merchPackRepository;

        public MerchPackController(IMediator mediator, StockApiGrpc.StockApiGrpcClient stockApiGrpcClient,
            IMapper mapper, IMerchPackRepository merchPackRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _merchPackRepository = merchPackRepository;
        }

        [HttpPost("create")]
        public async Task<ActionResult<RequestMerchResponse>> RequestMerch(
            [FromBody] CreateMerchPackRequest request,
            CancellationToken token)
        {
            var items = _mapper.Map<IEnumerable<MerchPackItem>>(request.Items);
            var pack = new MerchPack(request.MerchType);
            foreach (var merchPackItem in items)
            {
                pack.AddPosition(merchPackItem);
            }

            var response = await _merchPackRepository.Create(pack, token);
            return Ok(response);
        }
    }
}