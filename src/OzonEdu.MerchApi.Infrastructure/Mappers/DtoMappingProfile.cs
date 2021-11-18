using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Infrastructure.Mappers
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<EmployeeDto, Employee>().ConstructUsing(e =>
                new Employee(e.Id, Email.Create(e.Email),
                    PersonName.Create(e.FirstName, e.LastName),
                    e.ClothingSize.HasValue ? ClothingSize.ParseFromInt(e.ClothingSize.Value) : null,
                    e.Height.HasValue ? new Height(e.Height.Value) : null));

            CreateMap<MerchItemDto, MerchItem>().ConstructUsing(i =>
                new MerchItem(new Sku(i.Sku.Value), new Name(i.Name.Value),
                    new MerchCategory(i.Category.Id, i.Category.Name))).IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<MerchPackDto, MerchPack>()
                .ConstructUsing((m, ctx) => new MerchPack(m.Id, (MerchType)m.Type, GetPositions(m, ctx)))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<MerchRequestDto, MerchRequest>().ConstructUsing(d =>
                MerchRequest.Create(
                    d.Id,
                    new EmployeeId(d.EmployeeId),
                    MerchRequestMode.Parse(d.Mode),
                    d.StartedAt,
                    Email.Create(d.ManagerEmail),
                    (MerchType)d.RequestedMerchType,
                    MerchRequestStatus.Parse(d.Status),
                    d.ReservedAt)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            ;
        }

        private IEnumerable<MerchItem> GetPositions(MerchPackDto dto, ResolutionContext context)
        {
            var positionDtos = JsonSerializer.Deserialize<MerchItemDto[]>(dto.Positions) ??
                               Array.Empty<MerchItemDto>();

            return context.Mapper.Map<IEnumerable<MerchItem>>(positionDtos);
        }
    }
}