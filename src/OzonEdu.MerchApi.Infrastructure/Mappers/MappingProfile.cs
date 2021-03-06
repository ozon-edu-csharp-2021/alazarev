using AutoMapper;
using CSharpCourse.Core.Lib.Events;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationEvent, EmployeeNotificationDomainEvent>()
                .ForMember(d => d.EmployeeName,
                    o => o.Ignore())
                .ForMember(d => d.MerchDeliveryPayload,
                    o =>
                        o.MapFrom(s => s.Payload as MerchDeliveryEventPayload));
        }
    }
}