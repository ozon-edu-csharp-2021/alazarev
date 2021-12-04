using System;
using System.Text.Json;
using AutoMapper;
using CSharpCourse.Core.Lib.Events;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.HttpModels;

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
                        o.MapFrom(s => ConvertPayload(s)));

            CreateMap<MerchPackItemModel, MerchPackItem>()
                .ConstructUsing((m, ctx) =>
                    new MerchPackItem(new ItemId(m.ItemId), new Name(""), new Quantity(m.Quantity)))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }

        private MerchDeliveryEventPayload ConvertPayload(NotificationEvent notificationEvent)
        {
            if (notificationEvent?.Payload is JsonElement json)
            {
                return json.Deserialize<MerchDeliveryEventPayload>();
            }

            return null;
        }
    }
}