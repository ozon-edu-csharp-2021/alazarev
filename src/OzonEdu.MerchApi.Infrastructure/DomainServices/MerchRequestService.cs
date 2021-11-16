using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Domain.Contracts.DomainServices.MerchRequestService;
using OzonEdu.MerchApi.Domain.Contracts.StockApiService;
using OzonEdu.MerchApi.Domain.Exceptions;
using OzonEdu.MerchApi.Infrastructure.Repositories;

namespace OzonEdu.MerchApi.Infrastructure.DomainServices
{
    public class MerchRequestService : IMerchRequestService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IMerchRequestRepository _merchRequestRepository;
        private readonly IStockApiService _stockApiService;

        public MerchRequestService(IEmployeeRepository employeeRepository, IMerchPackRepository merchPackRepository,
            IMerchRequestRepository merchRequestRepository, IStockApiService stockApiService)
        {
            _employeeRepository = employeeRepository;
            _merchPackRepository = merchPackRepository;
            _merchRequestRepository = merchRequestRepository;
            _stockApiService = stockApiService;
        }
        

        public async Task<IEnumerable<IMerchRequest>> GetMerchInfoAsync(Email employeeEmail,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.FindByEmailAsync(employeeEmail, cancellationToken) ??
                           throw new EmployeeNotFoundException(employeeEmail.Value);
            return await _merchRequestRepository.GetAllEmployeeRequestsAsync(employee.Id, cancellationToken);
        }
        

        public async Task<IMerchRequest> CreateMerchRequestAsync(Email employeeEmail, MerchType merchType,
            MerchRequestMode merchRequestMode,
            CancellationToken token = default)
        {
            //берем сотрудника
            var employee = await _employeeRepository.FindByEmailAsync(employeeEmail, token) ??
                           throw new EmployeeNotFoundException(employeeEmail.Value);

            //берем мерчпак
            var merchPack = await _merchPackRepository.GetByMerchType(merchType, token) ??
                            throw new MerchPackNotFoundException();

            //данный мерч доступен пользователю?
            var isMerchAvailable = await CheckIfMerchAvailableAsync(employee.Id, merchType, token);

            if (!isMerchAvailable) return null;

            //TODO удлить. Это только для фейк репозитория
            var id = FakeMerchRequestRepository.Items.Count > 0
                ? FakeMerchRequestRepository.Items.Max(i => i.Id) + 1
                : 1;

            //создаям заявку
            var request = MerchRequest.Create(id, employee, merchRequestMode, DateTimeOffset.UtcNow);
            //устанавливаем заявке мерчпак и стартуем
            request.StartWork(merchPack);

            //запрашиваем наличие стоков на складе
            var stockItemsAvailabilityResult = await _stockApiService.GetStockItemsAvailabilityAsync(
                request.Items.Select(i => i.Sku.Value), token);

            //обновляем статусы позиций в соответствии с наличием на складе
            request.UpdateItemStatusesFromStockAvailabilities(stockItemsAvailabilityResult);

            //если стоки в наличие (заявка не перешла в ожидание)
            if (request.Status.Equals(MerchRequestStatus.InProcess))
            {
                //резервируем
                var reserveResult =
                    await _stockApiService.ReserveStockItemsAsync(request.Items.Select(i => i.Sku.Value), token);

                //результат резервирования применяем к заявке
                request.Reserve(reserveResult, reserveResult ? DateTimeOffset.UtcNow : null);
            }

            return request;
        }

        public async Task<bool> CheckIfMerchAvailableAsync(int employeeId, MerchType merchType,
            CancellationToken token = default)
        {
            var employeeRequests = await _merchRequestRepository.GetAllEmployeeRequestsAsync(employeeId, token);

            return !employeeRequests.Any(r =>
                r.RequestedMerchType == merchType
                && r.EmployeeId.Value == employeeId
                && ((r.Status.Equals(MerchRequestStatus.Reserved)
                     && r.ReservedAt.HasValue
                     && r.ReservedAt.Value.AddYears(1) > DateTimeOffset.UtcNow)
                    || r.Status.Equals(MerchRequestStatus.InProcess)
                    || r.Status.Equals(MerchRequestStatus.WaitingForSupply))
            );
        }
    }
}