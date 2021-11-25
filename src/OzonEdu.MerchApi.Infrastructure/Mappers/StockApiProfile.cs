using AutoMapper;
using OzonEdu.MerchApi.Infrastructure.StockApi;

namespace OzonEdu.MerchApi.Infrastructure.Mappers
{
    public class StockApiProfile : Profile
    {
        public StockApiProfile()
        {
            CreateMap<StockItemUnit, Domain.Contracts.StockApiService.StockItemUnit>();
        }
    }
}