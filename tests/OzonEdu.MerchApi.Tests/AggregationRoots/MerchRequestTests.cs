using System;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Events;
using OzonEdu.MerchApi.Domain.Exceptions;
using Xunit;

namespace OzonEdu.MerchApi.Tests.AggregationRoots
{
    public class MerchRequestTests
    {
        public static object[][] Create_IncorrectStartAt_Data =
        {
            new object[] { default(DateTimeOffset) },
            new object[] { DateTimeOffset.UtcNow.AddDays(1) }
        };

        [Fact]
        public void Create_StatusIsCreated()
        {
            var request = Create();
            Assert.Equal(request.Status, MerchRequestStatus.Created);
        }

        [Fact]
        public void StartWork_StatusIsInProcess()
        {
            var request = Create();
            StartWork(request);
            Assert.Equal(request.Status, MerchRequestStatus.InProcess);
            Assert.Contains(request.DomainEvents,
                notification => notification.GetType() == typeof(RequestProcessedEvent));
        }

        [Fact]
        public void CheckWithStock_StatusWhenAllInStock()
        {
            var request = Create();
            StartWork(request);
            CheckWithStockWhenAllInStock(request);
            Assert.Equal(request.Status, MerchRequestStatus.InProcess);
            Assert.All(request.Items, item => Assert.Equal(item.Status, RequestMerchItemStatus.InStock));
        }

        [Fact]
        public void CheckWithStock_StatusWhenAllOutStock()
        {
            var request = Create();
            StartWork(request);
            request.UpdateItemStatusesFromStockAvailabilities(request.Items.Select(i => new StockItem
            {
                SkuId = i.Sku.Value,
                Quantity = 0
            }));
            Assert.Equal(request.Status, MerchRequestStatus.WaitingForSupply);
            Assert.Contains(request.DomainEvents,
                notification => notification.GetType() == typeof(RequestWaitingForSupplyEvent));
            Assert.All(request.Items, item => Assert.Equal(item.Status, RequestMerchItemStatus.WaitingForSupply));
        }

        [Fact]
        public void Reserve_StatusWhenAllInStock()
        {
            var reservedAt = DateTimeOffset.UtcNow;
            var request = Create();
            StartWork(request);
            CheckWithStockWhenAllInStock(request);
            request.Reserve(true, reservedAt);

            Assert.Equal(request.Status, MerchRequestStatus.Reserved);
            Assert.Contains(request.DomainEvents,
                notification => notification.GetType() == typeof(RequestReservedEvent));
            Assert.Equal(request.ReservedAt, reservedAt);
        }

        [Fact]
        public void Reserve_StatusWhenError()
        {
            var request = Create();
            StartWork(request);
            CheckWithStockWhenAllInStock(request);
            request.Reserve(false);
            Assert.Contains(request.DomainEvents,
                notification => notification.GetType() == typeof(RequestOnErrorEvent));
            Assert.Equal(request.Status, MerchRequestStatus.Error);
        }

        [Theory]
        [MemberData(nameof(Create_IncorrectStartAt_Data))]
        public void Create_IncorrectStartAt(DateTimeOffset startedAt)
        {
            var employee = new Employee(Email.Create("qwe@qwe.ru"), PersonName.Create("Alex", "Lazarev"), null, null);

            Assert.Throws<IncorrectMerchRequestException>(() => MerchRequest.Create(
                employee,
                MerchRequestMode.ByRequest,
                startedAt));
        }

        [Fact]
        public void Reserve_WhenIncorrectStatus()
        {
            var request = Create();
            Assert.Throws<IncorrectRequestStatusException>(() => { request.Reserve(false); });
        }

        private MerchRequest Create()
        {
            var employee = new Employee(Email.Create("qwe@qwe.ru"), PersonName.Create("Alex", "Lazarev"), null, null);
            var request = MerchRequest.Create(
                employee,
                MerchRequestMode.ByRequest,
                new DateTimeOffset(2001, 10, 10, 0, 0, 0, TimeSpan.Zero));
            return request;
        }

        private void StartWork(MerchRequest request)
        {
            var merchPack = new MerchPack(MerchType.WelcomePack, new HumanResourceManagerId(0));
            merchPack.AddPosition(new MerchItem(new Sku(1), new Name("TShirt"), MerchCategory.TShirt));
            merchPack.AddPosition(new MerchItem(new Sku(2), new Name("Bag"), MerchCategory.Bag));
            merchPack.AddPosition(new MerchItem(new Sku(3), new Name("Socks"), MerchCategory.Socks));

            request.StartWork(merchPack);
        }

        private void CheckWithStockWhenAllInStock(MerchRequest request)
        {
            request.UpdateItemStatusesFromStockAvailabilities(request.Items.Select(i => new StockItem
            {
                SkuId = i.Sku.Value,
                Quantity = 10
            }));
        }
    }
}