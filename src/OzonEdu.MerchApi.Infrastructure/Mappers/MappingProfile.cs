using AutoMapper;
using CSharpCourse.Core.Lib.Events;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.Events;

namespace OzonEdu.MerchApi.Infrastructure.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationEvent, EmployeeNotificationDomainEvent>()
                .ForMember(d => d.Email,
                    o =>
                        o.MapFrom(s => Email.Create(s.EmployeeEmail)))
                .ForMember(d => d.EmployeeName,
                    o =>
                        o.MapFrom(s => PersonName.ParseFromFullName(s.EmployeeName)))
                .ForMember(d => d.MerchDeliveryPayload,
                    o =>
                        o.MapFrom(s => s.Payload as MerchDeliveryEventPayload));
        }
    }
}