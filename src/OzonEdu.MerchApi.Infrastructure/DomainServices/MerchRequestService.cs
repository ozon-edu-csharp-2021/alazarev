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


        public async Task CheckStockAsync(MerchRequest request, CancellationToken cancellationToken = default)
        {
            var skus = request.Items
                .Where(i => !i.Status.Equals(RequestMerchItemStatus.InStock))
                .Select(i => i.Sku.Value);
            var result = await _stockApiService.CheckStockItemsAsync(skus, cancellationToken);
            request.CheckWithStock(result);
        }

        public async Task<IEnumerable<MerchRequest>> GetMerchInfoAsync(string employeeEmail,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.FindByEmailAsync(employeeEmail, cancellationToken) ??
                           throw new EmployeeNotFoundException(employeeEmail);
            return await _merchRequestRepository.GetAllEmployeeRequestsAsync(employee.Id, cancellationToken);
        }

        public async Task ReserveStockAsync(MerchRequest request, CancellationToken cancellationToken = default)
        {
            var skus = request.Items.Select(i => i.Sku.Value);
            var result = await _stockApiService.ReserveStockItemsAsync(skus, cancellationToken);
            
            request.Reserve(result, result ? DateTimeOffset.UtcNow : null);
        }

        public async Task<MerchRequest> CreateMerchRequestAsync(string employeeEmail, MerchType merchType,
            MerchRequestMode merchRequestMode,
            CancellationToken token = default)
        {
            var employee = await _employeeRepository.FindByEmailAsync(employeeEmail, token) ??
                           throw new EmployeeNotFoundException(employeeEmail);

            var merchPack = await _merchPackRepository.GetByMerchType(merchType, token) ??
                            throw new MerchPackNotFoundException();

            var isMerchAvailable = await CheckIfMerchAvailableAsync(employee.Id, merchType, token);

            if (!isMerchAvailable) return null;

            var id = FakeMerchRequestRepository.Items.Count > 0
                ? FakeMerchRequestRepository.Items.Max(i => i.Id) + 1
                : 1;

            var request = MerchRequest.Create(id, employee, merchRequestMode, DateTimeOffset.UtcNow);
            request.StartWork(merchPack);

            await CheckStockAsync(request, token);

            if (request.Status.Equals(MerchRequestStatus.InProcess))
            {
                await ReserveStockAsync(request, token);
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