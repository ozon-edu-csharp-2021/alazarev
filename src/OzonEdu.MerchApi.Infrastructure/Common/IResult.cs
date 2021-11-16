namespace OzonEdu.MerchApi.Infrastructure.Common
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }
}