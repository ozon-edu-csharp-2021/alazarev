using System;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class MerchRequest : Entity, IAggregationRoot, IMerchRequest
    {
        private List<RequestMerchItem> _items = new();
        public DateTimeOffset StartedAt { get; private set; }

        public Email ManagerEmail { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchRequestStatus Status { get; private set; }
        public MerchType RequestedMerchType { get; private set; }
        public MerchRequestMode Mode { get; private set; }
        public DateTimeOffset? ReservedAt { get; private set; }

        public IReadOnlyCollection<RequestMerchItem> Items => _items.AsReadOnly();

        private MerchRequest(EmployeeId employeeId, MerchRequestMode mode,
            DateTimeOffset startedAt, Email managerEmail)
        {
            //если не инициализировали
            if (startedAt == default) throw new IncorrectMerchRequestException();
            //если дата указана в будущем
            if (startedAt > DateTimeOffset.UtcNow) throw new IncorrectMerchRequestException();
            EmployeeId = employeeId;
            Mode = mode;
            StartedAt = startedAt;
            ManagerEmail = managerEmail;
            Status = MerchRequestStatus.Created;
        }

        public static MerchRequest Create(EmployeeId employeeId, MerchRequestMode mode,
            DateTimeOffset startedAt, Email managerEmail) => new(employeeId, mode, startedAt, managerEmail);

        public static MerchRequest Create(int id, EmployeeId employeeId, MerchRequestMode mode,
            DateTimeOffset startedAt, Email managerEmail) => new(employeeId, mode, startedAt, managerEmail) { Id = id };

        public static MerchRequest Create(int id, EmployeeId employeeId, MerchRequestMode mode,
            DateTimeOffset startedAt, Email managerEmail, MerchType requestedMerchType, MerchRequestStatus status, DateTimeOffset? reservedAt) =>
            new(employeeId, mode, startedAt, managerEmail)
            {
                Id = id,
                Status = status,
                ReservedAt = reservedAt,
                RequestedMerchType = requestedMerchType
            };

        public void StartWork(MerchPack merchPack)
        {
            if (!Equals(Status, MerchRequestStatus.Created))
            {
                throw new IncorrectRequestStatusException();
            }

            RequestedMerchType = merchPack.Type;

            _items.AddRange(merchPack.Positions.Select(i => new RequestMerchItem(i.Sku)));
            Status = MerchRequestStatus.InProcess;
            AddDomainEvent(new RequestProcessedEvent(Id));
        }

        public void UpdateItemStatusesFromStockAvailabilities(IEnumerable<StockItem> stockItems)
        {
            if (!Equals(Status, MerchRequestStatus.InProcess))
            {
                throw new IncorrectRequestStatusException();
            }

            foreach (var requestMerchItem in _items)
            {
                var stockItem = stockItems.FirstOrDefault(i => i.SkuId == requestMerchItem.Sku.Value);
                if (stockItem is { Quantity: > 0 })
                {
                    requestMerchItem.ChangeStatus(RequestMerchItemStatus.InStock);
                }
                else
                {
                    requestMerchItem.ChangeStatus(RequestMerchItemStatus.WaitingForSupply);
                    Status = MerchRequestStatus.WaitingForSupply;
                }
            }

            if (Status.Equals(MerchRequestStatus.WaitingForSupply))
            {
                AddDomainEvent(new RequestWaitingForSupplyEvent(this));
            }
        }

        public void UpdateItemStatusesFromSupply(IEnumerable<StockItem> stockItems)
        {
            if (!Equals(Status, MerchRequestStatus.WaitingForSupply))
            {
                throw new IncorrectRequestStatusException();
            }

            var waitingForSupplyItems = _items
                .Where(i => i.Status.Equals(RequestMerchItemStatus.WaitingForSupply)).ToArray();
            var everythingIsPresent = true;
            foreach (var requestMerchItem in waitingForSupplyItems)
            {
                var stockItem = stockItems.FirstOrDefault(i => i.SkuId == requestMerchItem.Sku.Value);
                if (stockItem is { Quantity: > 0 })
                {
                    requestMerchItem.ChangeStatus(RequestMerchItemStatus.InStock);
                    stockItem.Quantity--;
                }
                else
                {
                    everythingIsPresent = false;
                }
            }

            if (everythingIsPresent)
            {
                if (Mode.Equals(MerchRequestMode.ByRequest))
                {
                    AddDomainEvent(new MerchInStockEvent(EmployeeId, new MerchRequestId(Id)));
                    Status = MerchRequestStatus.Informed;
                }
                else
                {
                    Status = MerchRequestStatus.InProcess;
                    AddDomainEvent(new RequestProcessedEvent(Id));
                }
            }
        }

        public void Reserve(bool reserved, DateTimeOffset? reservedAt = null)
        {
            if (!Equals(Status, MerchRequestStatus.InProcess))
            {
                throw new IncorrectRequestStatusException();
            }

            if (reserved)
            {
                Status = MerchRequestStatus.Reserved;
                ReservedAt = reservedAt;
                AddDomainEvent(new RequestReservedEvent(Id));
            }
            else
            {
                Status = MerchRequestStatus.Error;
                AddDomainEvent(new RequestOnErrorEvent(Id));
            }
        }
    }
}