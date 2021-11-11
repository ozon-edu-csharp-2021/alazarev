using System;
using System.Collections.Generic;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public interface IMerchRequest
    {
        DateTimeOffset StartedAt { get; }
        HumanResourceManagerId ManagerId { get; }
        EmployeeId EmployeeId { get; }
        MerchRequestStatus Status { get; }
        MerchType RequestedMerchType { get; }
        MerchRequestMode Mode { get; }
        DateTimeOffset? ReservedAt { get; }

        IReadOnlyCollection<RequestMerchItem> Items { get; }

        void StartWork(MerchPack merchPack);

        void CheckWithStock(IEnumerable<StockItem> stockItems);

        void CheckWithSupply(IEnumerable<StockItem> stockItems);

        void Reserve(bool reserved, DateTimeOffset? reservedAt = null);
    }
}