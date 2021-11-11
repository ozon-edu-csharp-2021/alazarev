using System.Threading.Tasks;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Bus
{
    public class FakeMessageBus : IMessageBus
    {
        public Task NotifyAsync(EmailMessage emailMessage) => Task.CompletedTask;
    }
}