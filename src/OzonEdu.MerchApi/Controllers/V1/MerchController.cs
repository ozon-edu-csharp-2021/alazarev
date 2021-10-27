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
        public async Task<ActionResult<RequestMerchResponseViewModel>> RequestMerch(
            [FromBody] RequestMerchViewModel request,
            CancellationToken token)
        {
            var status = await _merchService.RequestMerchAsync(request.EmployeeId, request.MerchType, token);

            var statusViewModel = ViewModelMapper.RequestMerchStatusToViewModelStatus(status);

            return Ok(new RequestMerchResponseViewModel(statusViewModel));
        }

        [HttpGet("getinfo/{employeeId:int}")]
        public async Task<ActionResult<GetReceivingMerchInfoResponseViewModel>> GetReceivingMerchInfo(int employeeId,
            CancellationToken token)
        {
            var items = await _merchService.GetReceivingMerchInfoAsync(employeeId, token);
            var viewModelItems = items.Select(ViewModelMapper.MerchInfoModelToViewModel).ToArray();
            return Ok(new GetReceivingMerchInfoResponseViewModel(viewModelItems));
        }
    }
}