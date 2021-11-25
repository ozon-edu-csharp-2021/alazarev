using System;
using System.Threading;
using CSharpCourse.Core.Lib.Enums;
using Moq;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Handlers.Commands;
using OzonEdu.MerchApi.Infrastructure.Persistence;
using OzonEdu.MerchApi.Infrastructure.Persistence.Repositories;
using OzonEdu.MerchApi.Infrastructure.Repositories;
using Xunit;

namespace OzonEdu.MerchApi.Tests.CommandHandlers
{
    public class CreateMerchRequestCommandHandlerTests
    {
        // [Fact]
        // public async void Handle_WhenSuccess()
        // {
        //     var validator = new CreateMerchRequestCommandValidator();
        //     var merchRequestService = new Mock<IMerchRequestService>();
        //     merchRequestService
        //         .Setup(x
        //             => x.CreateMerchRequestAsync(It.IsAny<EmployeeEmail>(), It.IsAny<ManagerEmail>(),
        //                 It.IsAny<MerchType>(),
        //                 It.IsAny<MerchRequestMode>(), It.IsAny<CancellationToken>()))
        //         .ReturnsAsync(MerchRequest.Create(EmployeeEmail.Create("qwe@qwe.ru"),
        //             ManagerEmail.Create("manager@ozon.ru"), MerchRequestMode.Auto,
        //             DateTimeOffset.UtcNow));
        //
        //     var fakeMerchRequestRepository = new FakeMerchRequestRepository(new FakeUnitOfWork());
        //
        //     var handler = new CreateMerchRequestCommandHandler(validator, fakeMerchRequestRepository,
        //         merchRequestService.Object);
        //
        //     var result = await handler.Handle(
        //         new CreateMerchRequestCommand("zxcvb@asdasd.ru", "zxcvb@asdasd.ru", ClothingSize.M,
        //             MerchType.VeteranPack,
        //             MerchRequestMode.ByRequest),
        //         CancellationToken.None);
        //
        //     Assert.True(result.IsSuccess);
        // }

        [Fact]
        public async void Handle_WhenFail()
        {
            var validator = new CreateMerchRequestCommandValidator();

            var merchPackRepository = new Mock<IMerchPackRepository>();
            merchPackRepository
                .Setup(x
                    => x.GetByMerchTypeAsync(It.IsAny<MerchType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as MerchPack);

            var fakeMerchRequestRepository = new FakeMerchRequestRepository(new FakeUnitOfWork());

            var handler = new CreateMerchRequestCommandHandler(validator, fakeMerchRequestRepository,
                merchPackRepository.Object, null);

            var result = await handler.Handle(
                new CreateMerchRequestCommand("zxcvb@asdasd.ru", "zxcvb@asdasd.ru", ClothingSize.M,
                    MerchType.VeteranPack,
                    MerchRequestMode.ByRequest),
                CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async void Handle_ValidationError()
        {
            var validator = new CreateMerchRequestCommandValidator();
            var handler = new CreateMerchRequestCommandHandler(validator, null,
                null, null);

            var result = await handler.Handle(
                new CreateMerchRequestCommand("incorrect", "coorect@email.yes", ClothingSize.M, MerchType.VeteranPack,
                    MerchRequestMode.ByRequest),
                CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.StartsWith("Произошла ошибка валидации", result.Message);
        }
    }
}