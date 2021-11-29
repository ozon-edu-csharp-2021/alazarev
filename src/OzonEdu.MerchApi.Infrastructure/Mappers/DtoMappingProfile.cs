using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;
using ClothingSize = CSharpCourse.Core.Lib.Enums.ClothingSize;

namespace OzonEdu.MerchApi.Infrastructure.Mappers
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<RequestMerchItemDto, RequestMerchItem>().ConstructUsing(i =>
                new RequestMerchItem(new Sku(i.Sku.Value),
                    new Quantity(i.Quantity.Value))).IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<MerchPackItemDto, MerchPackItem>().ConstructUsing(i =>
                new MerchPackItem(new ItemId(i.ItemId.Value), new Name(i.Name.Value),
                    new Quantity(i.Quantity.Value))).IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<MerchPackDto, MerchPack>()
                .ConstructUsing((m, ctx) => new MerchPack(m.Id, (MerchType)m.Type, GetPositions(m, ctx)))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<MerchRequestDto, MerchRequest>().ConstructUsing((d, ctx) =>
                MerchRequest.Create(
                    d.Id,
                    EmployeeEmail.Create(d.EmployeeEmail),
                    ManagerEmail.Create(d.ManagerEmail),
                    (ClothingSize)d.ClothingSize,
                    MerchRequestMode.Parse(d.Mode),
                    d.StartedAt,
                    (MerchType)d.RequestedMerchType,
                    MerchRequestStatus.Parse(d.Status),
                    d.ReservedAt, GetItems(d, ctx))).IgnoreAllPropertiesWithAnInaccessibleSetter();
            ;
        }

        private IEnumerable<MerchPackItem> GetPositions(MerchPackDto dto, ResolutionContext context)
        {
            var positionDtos = JsonSerializer.Deserialize<MerchPackItemDto[]>(dto.Positions) ??
                               Array.Empty<MerchPackItemDto>();

            return context.Mapper.Map<IEnumerable<MerchPackItem>>(positionDtos);
        }

        private IEnumerable<RequestMerchItem> GetItems(MerchRequestDto dto, ResolutionContext context)
        {
            var itemsDtos = JsonSerializer.Deserialize<RequestMerchItemDto[]>(dto.RequestMerch) ??
                            Array.Empty<RequestMerchItemDto>();

            return context.Mapper.Map<IEnumerable<RequestMerchItem>>(itemsDtos);
        }
    }
}