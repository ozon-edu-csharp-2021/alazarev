using System;
using System.Threading;
using CSharpCourse.Core.Lib.Enums;
using Moq;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Infrastructure.Commands.CreateMerchRequest;
using OzonEdu.MerchApi.Infrastructure.Handlers.Commands;
using OzonEdu.MerchApi.Infrastructure.Repositories;
using OzonEdu.MerchApi.Infrastructure.Uow;
using Xunit;

namespace OzonEdu.MerchApi.Tests.CommandHandlers
{
    public class CreateMerchRequestCommandHandlerTests
    {
        [Fact]
        public async void Handle_WhenSuccess()
        {
            var employee = new Employee(Email.Create("qwe@qwe.ru"), PersonName.Create("Alex", "Lazarev"), null, null);
            var validator = new CreateMerchRequestCommandValidator();
            var merchRequestService = new Mock<IMerchRequestService>();
            merchRequestService
                .Setup(x
                    => x.CreateMerchRequestAsync(It.IsAny<Email>(), It.IsAny<MerchType>(),
                        It.IsAny<MerchRequestMode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(MerchRequest.Create(employee, MerchRequestMode.Auto, DateTimeOffset.UtcNow));

            var fakeMerchRequestRepository = new FakeMerchRequestRepository(new FakeUnitOfWork());

            var handler = new CreateMerchRequestCommandHandler(validator, fakeMerchRequestRepository,
                merchRequestService.Object);

            var result = await handler.Handle(
                new CreateMerchRequestCommand("zxcvb@asdasd.ru", MerchType.VeteranPack, MerchRequestMode.ByRequest),
                CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Handle_WhenFail()
        {
            var validator = new CreateMerchRequestCommandValidator();
            var merchRequestService = new Mock<IMerchRequestService>();
            merchRequestService
                .Setup(x
                    => x.CreateMerchRequestAsync(It.IsAny<Email>(), It.IsAny<MerchType>(),
                        It.IsAny<MerchRequestMode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as MerchRequest);

            var fakeMerchRequestRepository = new FakeMerchRequestRepository(new FakeUnitOfWork());

            var handler = new CreateMerchRequestCommandHandler(validator, fakeMerchRequestRepository,
                merchRequestService.Object);

            var result = await handler.Handle(
                new CreateMerchRequestCommand("zxcvb@asdasd.ru", MerchType.VeteranPack, MerchRequestMode.ByRequest),
                CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async void Handle_ValidationError()
        {
            var validator = new CreateMerchRequestCommandValidator();

            var handler = new CreateMerchRequestCommandHandler(validator, null,
                null);

            var result = await handler.Handle(
                new CreateMerchRequestCommand("incorrect", MerchType.VeteranPack, MerchRequestMode.ByRequest),
                CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.StartsWith("Произошла ошибка валидации",result.Message);
        }
    }
}