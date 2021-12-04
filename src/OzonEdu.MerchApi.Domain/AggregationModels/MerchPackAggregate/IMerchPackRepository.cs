using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.Contracts;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public interface IMerchPackRepository : IRepository<MerchPack>
    {
        Task<MerchPack> GetByMerchTypeAsync(MerchType merchType, CancellationToken cancellationToken = default);
        
        Task<MerchPack> Create(MerchPack merchPack, CancellationToken cancellationToken = default);
    }
}