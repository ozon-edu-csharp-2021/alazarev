using System;
using System.Collections.Generic;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public interface IMerchRequest : IEntity
    {
        DateTimeOffset StartedAt { get; }
        public Email ManagerEmail { get; }
        EmployeeId EmployeeId { get; }
        MerchRequestStatus Status { get; }
        MerchType RequestedMerchType { get; }
        MerchRequestMode Mode { get; }
        DateTimeOffset? ReservedAt { get; }

        IReadOnlyCollection<RequestMerchItem> Items { get; }

        void StartWork(MerchPack merchPack);

        void UpdateItemStatusesFromStockAvailabilities(IEnumerable<StockItem> stockItems);

        void UpdateItemStatusesFromSupply(IEnumerable<StockItem> stockItems);

        void Reserve(bool reserved, DateTimeOffset? reservedAt = null);
    }
}