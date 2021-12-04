using System;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate
{
    public class MerchRequest : Entity, IAggregationRoot
    {
        private List<RequestMerchItem> _items = new();
        public DateTimeOffset StartedAt { get; private set; }
        public ManagerEmail ManagerEmail { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchRequestStatus Status { get; private set; }
        public MerchType RequestedMerchType { get; private set; }
        public MerchRequestMode Mode { get; private set; }
        public DateTimeOffset? ReservedAt { get; private set; }

        public IReadOnlyCollection<RequestMerchItem> Items => _items.AsReadOnly();

        private MerchRequest(EmployeeId employeeId, ManagerEmail managerEmail,
            MerchRequestMode mode,
            DateTimeOffset startedAt)
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

        public static MerchRequest Create(EmployeeId employeeId, ManagerEmail managerEmail, MerchRequestMode mode,
            DateTimeOffset startedAt) => new(employeeId, managerEmail, mode, startedAt);

        public static MerchRequest Create(int id, EmployeeId employeeId, ManagerEmail managerEmail,
            MerchRequestMode mode,
            DateTimeOffset startedAt) => new(employeeId, managerEmail, mode, startedAt) { Id = id };

        public static MerchRequest Create(int id, EmployeeId employeeId, ManagerEmail managerEmail,
            MerchRequestMode mode,
            DateTimeOffset startedAt, MerchType requestedMerchType, MerchRequestStatus status,
            DateTimeOffset? reservedAt,
            IEnumerable<RequestMerchItem> items) =>
            new(employeeId, managerEmail, mode, startedAt)
            {
                Id = id,
                Status = status,
                ReservedAt = reservedAt,
                RequestedMerchType = requestedMerchType,
                _items = new List<RequestMerchItem>(items)
            };

        public void StartWork(MerchPack merchPack, IEnumerable<StockItemUnit> merchPackSkusInStock)
        {
            if (!Equals(Status, MerchRequestStatus.Created))
            {
                throw new IncorrectRequestStatusException();
            }

            RequestedMerchType = merchPack.Type;
            var isValid = true;
            foreach (var merchPackPosition in merchPack.Positions)
            {
                var positionInStock =
                    merchPackSkusInStock.FirstOrDefault(i => i.ItemTypeId == merchPackPosition.ItemId.Value);

                if (positionInStock == null) throw new Exception("Sku not found");

                if (positionInStock.Quantity < merchPackPosition.Quantity.Value)
                {
                    isValid = false;
                }

                _items.Add(new RequestMerchItem(new Sku(positionInStock.Sku),
                    new Quantity(merchPackPosition.Quantity.Value)));
            }

            if (isValid)
            {
                Status = MerchRequestStatus.InProcess;
                AddDomainEvent(new RequestProcessedEvent(Id));
            }
            else
            {
                Status = MerchRequestStatus.WaitingForSupply;
                AddDomainEvent(new RequestWaitingForSupplyEvent(this));
            }
        }

        public void Complete(bool reserved, DateTimeOffset? reservedAt = null)
        {
            if (!Equals(Status, MerchRequestStatus.InProcess) && !Equals(Status, MerchRequestStatus.WaitingForSupply))
            {
                throw new IncorrectRequestStatusException();
            }

            if (reserved)
            {
                Status = MerchRequestStatus.Reserved;
                ReservedAt = reservedAt;
                AddDomainEvent(new RequestReservedEvent(this));
            }
            else
            {
                if (Equals(Status, MerchRequestStatus.WaitingForSupply) && Equals(Mode, MerchRequestMode.ByRequest))
                {
                    AddDomainEvent(new MerchInStockEvent(this));
                    Status = MerchRequestStatus.Informed;
                }
                else
                {
                    Status = MerchRequestStatus.Error;
                    AddDomainEvent(new RequestOnErrorEvent(Id));
                }
            }
        }

        public void Cancel()
        {
            Status = MerchRequestStatus.Canceled;
            AddDomainEvent(new MerchRequestCanceledEvent(this));
        }
    }
}