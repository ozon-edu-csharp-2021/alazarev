using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using Moq;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Infrastructure.DomainServices;
using Xunit;

namespace OzonEdu.MerchApi.Tests.DomainServices
{
    public class MerchRequestServiceTests
    {
        [Fact]
        public async Task CheckIfMerchAvailableAsync_WhenNoRequests()
        {
            var employeeRepository = new Mock<IEmployeeRepository>();
            var merchPackRepository = new Mock<IMerchPackRepository>();
            var merchRequestRepository = new Mock<IMerchRequestRepository>();
            var stockApiService = new Mock<IStockApiService>();

            merchRequestRepository
                .Setup(x
                    => x.GetAllEmployeeRequestsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MerchRequest[] { });


            var merchRequestService = new MerchRequestService(employeeRepository.Object, merchPackRepository.Object,
                merchRequestRepository.Object, stockApiService.Object);

            Assert.True(await merchRequestService.CheckIfMerchAvailableAsync(1, MerchType.VeteranPack));
        }

        [Fact]
        public async Task CheckIfMerchAvailableAsync_WhenMoreThanOneYearAgo()
        {
            var employee = new Employee(Email.Create("qwe@qwe.ru"), PersonName.Create("Alex", "Lazarev"), null, null);
            var startedAt = new DateTimeOffset(2001, 10, 10, 0, 0, 0, TimeSpan.Zero);
            var merchRequestRepository = new Mock<IMerchRequestRepository>();
            var merchRequest = new Mock<MerchRequest>(employee, MerchRequestMode.ByRequest, startedAt);

            merchRequest.SetupGet(r => r.RequestedMerchType).Returns(MerchType.VeteranPack);
            merchRequest.SetupGet(r => r.Status).Returns(MerchRequestStatus.Reserved);
            merchRequest.SetupGet(r => r.ReservedAt).Returns(DateTimeOffset.UtcNow.AddYears(-2));

            merchRequestRepository
                .Setup(x
                    => x.GetAllEmployeeRequestsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MerchRequest[] {merchRequest.Object});


            var merchRequestService = new MerchRequestService(null, null,
                merchRequestRepository.Object, null);

            Assert.True(await merchRequestService.CheckIfMerchAvailableAsync(1, MerchType.VeteranPack));
        }

        [Fact]
        public async Task CheckIfMerchAvailableAsync_WhenLessThanOneYearAgo()
        {
            var employee = new Employee(1, Email.Create("qwe@qwe.ru"), PersonName.Create("Alex", "Lazarev"), null,
                null);
            var startedAt = new DateTimeOffset(2001, 10, 10, 0, 0, 0, TimeSpan.Zero);
            var merchRequestRepository = new Mock<IMerchRequestRepository>();
            var merchRequest = new Mock<MerchRequest>(employee, MerchRequestMode.ByRequest, startedAt);

            merchRequest.SetupGet(r => r.RequestedMerchType).Returns(MerchType.VeteranPack);
            merchRequest.SetupGet(r => r.Status).Returns(MerchRequestStatus.Reserved);
            merchRequest.SetupGet(r => r.ReservedAt).Returns(DateTimeOffset.UtcNow.AddMonths(-10));

            merchRequestRepository
                .Setup(x
                    => x.GetAllEmployeeRequestsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MerchRequest[] {merchRequest.Object});

            var merchRequestService = new MerchRequestService(null, null,
                merchRequestRepository.Object, null);

            Assert.False(await merchRequestService.CheckIfMerchAvailableAsync(1, MerchType.VeteranPack));
        }

        [Fact]
        public async Task CreateMerchRequestAsync_EmployeeNotFound()
        {
            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository
                .Setup(x
                    => x.FindByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee) null);


            var merchRequestService = new MerchRequestService(employeeRepository.Object, null,
                null, null);

            await Assert.ThrowsAsync<EmployeeNotFoundException>(async () =>
                await merchRequestService.CreateMerchRequestAsync(
                    "not@found_emplo.ee", MerchType.WelcomePack,
                    MerchRequestMode.ByRequest));
        }

        [Fact]
        
        public async Task CreateMerchRequestAsync_MerchPackNotFound()
        {
            var employee = new Employee(1, Email.Create("employee@who.ok"), PersonName.Create("Alex", "Lazarev"), null,
                null);

            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository
                .Setup(x
                    => x.FindByEmailAsync(employee.Email.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            var merchPackRepository = new Mock<IMerchPackRepository>();
            merchPackRepository
                .Setup(x
                    => x.GetByMerchType(It.IsAny<MerchType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as MerchPack);

            var merchRequestService = new MerchRequestService(employeeRepository.Object, merchPackRepository.Object,
                null, null);

            await Assert.ThrowsAsync<MerchPackNotFoundException>(async () =>
                await merchRequestService.CreateMerchRequestAsync(
                    "employee@who.ok", MerchType.WelcomePack,
                    MerchRequestMode.ByRequest));
        }
    }
}