using System;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
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
            StartWorkOk(request);
            Assert.Equal(request.Status, MerchRequestStatus.WaitingForSupply);
        }


        [Fact]
        public void Reserve_StatusWhenAllInStock()
        {
            var reservedAt = DateTimeOffset.UtcNow;
            var request = Create();
            StartWorkOk(request);
            request.Complete(true, reservedAt);

            Assert.Equal(request.Status, MerchRequestStatus.Reserved);
            Assert.Contains(request.DomainEvents,
                notification => notification.GetType() == typeof(RequestReservedEvent));
            Assert.Equal(request.ReservedAt, reservedAt);
        }

        [Fact]
        public void Reserve_ErrorWhenSkuNotFound()
        {
            var request = Create();

            var exception = Assert.Throws<Exception>(() => { StartWork(request); });
            Assert.Equal("Sku not found", exception.Message);
        }

        [Theory]
        [MemberData(nameof(Create_IncorrectStartAt_Data))]
        public void Create_IncorrectStartAt(DateTimeOffset startedAt)
        {
            Assert.Throws<IncorrectMerchRequestException>(() => MerchRequest.Create(
                EmployeeEmail.Create("vasya@ozon.ru"),
                ManagerEmail.Create("manager@ozon.ru"),
                ClothingSize.L,
                MerchRequestMode.ByRequest,
                startedAt
            ));
        }

        [Fact]
        public void Reserve_WhenIncorrectStatus()
        {
            var request = Create();
            Assert.Throws<IncorrectRequestStatusException>(() => { request.Complete(false); });
        }

        private MerchRequest Create()
        {
            var request = MerchRequest.Create(
                EmployeeEmail.Create("qwe@qwe.ru"),
                ManagerEmail.Create("manager@ozon.ru"),
                ClothingSize.S,
                MerchRequestMode.ByRequest,
                new DateTimeOffset(2001, 10, 10, 0, 0, 0, TimeSpan.Zero));
            return request;
        }

        private void StartWork(MerchRequest request)
        {
            var merchPack = new MerchPack(MerchType.WelcomePack);
            merchPack.AddPosition(new MerchPackItem(new ItemId(1), new Name("TShirt"), new Quantity(1)));
            merchPack.AddPosition(new MerchPackItem(new ItemId(2), new Name("Bag"), new Quantity(1)));
            merchPack.AddPosition(new MerchPackItem(new ItemId(3), new Name("Socks"), new Quantity(1)));

            request.StartWork(merchPack, new StockItemUnit[] { });
        }

        private void StartWorkOk(MerchRequest request)
        {
            var merchPack = new MerchPack(MerchType.WelcomePack);
            merchPack.AddPosition(new MerchPackItem(new ItemId(1), new Name("TShirt"), new Quantity(1)));
            merchPack.AddPosition(new MerchPackItem(new ItemId(2), new Name("Bag"), new Quantity(1)));
            merchPack.AddPosition(new MerchPackItem(new ItemId(3), new Name("Socks"), new Quantity(1)));

            request.StartWork(merchPack,
                new StockItemUnit[]
                {
                    new() { Sku = 1, ItemTypeId = 1 }, new() { Sku = 2, ItemTypeId = 2 },
                    new() { Sku = 3, ItemTypeId = 3 }
                });
        }
    }
}