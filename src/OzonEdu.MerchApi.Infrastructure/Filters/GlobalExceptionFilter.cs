using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OzonEdu.MerchApi.Infrastructure.Filters
{
    /// <summary>
    /// Глобальный exception filter, который отлавливает все необработанные исключения
    /// </summary>
    public sealed class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context) =>
            context.Result = new JsonResult(new
            {
                Name = context.Exception.GetType().FullName,
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
    }
}