using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Infrastructure.Repositories
{
    public class FakeMerchPackRepository : FakeRepository<MerchPack>, IMerchPackRepository
    {
        static FakeMerchPackRepository()
        {
            Items.Add(new MerchPack(MerchType.WelcomePack,
                new HumanResourceManagerId(1)));
            Items.Add(new MerchPack(MerchType.ConferenceListenerPack,
                new HumanResourceManagerId(2)));
        }

        public Task<MerchPack> GetByMerchType(MerchType merchType, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(p => p.Type == merchType));
        }

        public FakeMerchPackRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}