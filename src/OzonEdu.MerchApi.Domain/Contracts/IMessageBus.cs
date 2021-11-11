using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchApi.Domain.Contracts
{
    public interface IMessageBus
    {
        Task NotifyAsync(EmailMessage emailMessage);
    }
}