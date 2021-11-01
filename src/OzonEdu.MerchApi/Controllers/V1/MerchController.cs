using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchApi.HttpModels;
using OzonEdu.MerchApi.Mappers;
using OzonEdu.MerchApi.Services.Interfaces;

namespace OzonEdu.MerchApi.Controllers.V1
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class MerchController : ControllerBase
    {
        private readonly IMerchService _merchService;

        public MerchController(IMerchService merchService)
        {
            _merchService = merchService;
        }

        [HttpPost("request")]
        public async Task<ActionResult<RequestMerchResponse>> RequestMerch(
            [FromBody] RequestMerchRequest request,
            CancellationToken token)
        {
            var response = await _merchService.RequestMerchAsync(request, token);

            return Ok(response);
        }

        [HttpGet("getinfo/{employeeId:int}")]
        public async Task<ActionResult<GetReceivingMerchInfoResponse>> GetReceivingMerchInfo(
            GetReceivingMerchInfoRequest request,
            CancellationToken token)
        {
            var response = await _merchService.GetReceivingMerchInfoAsync(request, token);
            return Ok(response);
        }
    }
}