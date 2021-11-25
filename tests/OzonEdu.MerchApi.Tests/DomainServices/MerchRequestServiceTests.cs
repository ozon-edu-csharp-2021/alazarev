using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using Moq;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Handlers.Commands;
using Xunit;

namespace OzonEdu.MerchApi.Tests.DomainServices
{
    public class MerchRequestServiceTests
    {
        [Fact]
        public async Task CreateMerchRequestAsync_MerchPackNotFound()
        {
            var validator = new CreateMerchRequestCommandValidator();
            var merchPackRepository = new Mock<IMerchPackRepository>();
            merchPackRepository
                .Setup(x
                    => x.GetByMerchTypeAsync(It.IsAny<MerchType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as MerchPack);

            var merchRequestRepository = new Mock<IMerchRequestRepository>();
            merchRequestRepository
                .Setup(x
                    => x.GetAllEmployeeRequestsAsync(It.IsAny<EmployeeEmail>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MerchRequest[] { });

            var handler =
                new CreateMerchRequestCommandHandler(validator, merchRequestRepository.Object,
                    merchPackRepository.Object, null);

            var result = await handler.Handle(new CreateMerchRequestCommand("employee@who.ok",
                "employee@who.ok", ClothingSize.L,
                MerchType.WelcomePack,
                MerchRequestMode.ByRequest), CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}