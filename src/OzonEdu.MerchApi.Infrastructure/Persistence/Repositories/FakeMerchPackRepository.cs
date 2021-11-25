using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Persistence.Repositories;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeMerchPackRepository : FakeRepository<MerchPack>, IMerchPackRepository
    {
        static FakeMerchPackRepository()
        {
            Items.Add(new MerchPack(MerchType.WelcomePack));
            Items.Add(new MerchPack(MerchType.ConferenceListenerPack));
        }

        public Task<MerchPack> GetByMerchTypeAsync(MerchType merchType, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(p => p.Type == merchType));
        }

        public FakeMerchPackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}