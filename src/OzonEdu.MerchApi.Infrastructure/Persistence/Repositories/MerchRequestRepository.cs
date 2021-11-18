using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Npgsql;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Persistence.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;
using OzonEdu.MerchApi.Infrastructure.Repositories;
using OzonEdu.StockApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class MerchRequestRepository : IMerchRequestRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMapper _mapper;
        private const int Timeout = 5;

        public async Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(int employeeId,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT id, started_at, manager_email, employee_id, status, requested_merch_type, mode, reserved_at from merch_request where employee_id=@EmployeeId;";

            var parameters = new
            {
                EmployeeId = employeeId
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchRequestDtos = await connection.QueryAsync<MerchRequestDto>(commandDefinition);

            return _mapper.Map<IEnumerable<MerchRequest>>(merchRequestDtos);
        }

        public async Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsByModeAsync(MerchRequestMode mode,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT id, started_at, manager_email, employee_id, status, requested_merch_type, mode, reserved_at from merch_request where status=@Status;";

            var parameters = new
            {
                Status = MerchRequestStatus.WaitingForSupply
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchRequestDtos = await connection.QueryAsync<MerchRequestDto>(commandDefinition);

            return _mapper.Map<IEnumerable<MerchRequest>>(merchRequestDtos);
        }

        public async Task<MerchRequest> CreateAsync(MerchRequest request, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO merch_request (started_at, manager_email, employee_id, status, requested_merch_type,mode,reserved_at)
                VALUES (@StartAt, @ManagerEmail, @EmployeeId, @Status, @RequestedMerchType, @Mode, @ReservedAt);";

            var parameters = new
            {
                StartAt = request.StartedAt,
                ManagerEmail = request.ManagerEmail.Value,
                EmployeeId = request.EmployeeId.Value,
                Status = request.Status.Id,
                RequestedMerchType = (int)request.RequestedMerchType,
                Mode = request.Mode.Id,
                ReservedAt = request.ReservedAt
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(request);
            return request;
        }

        public MerchRequestRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker, IMapper mapper)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _mapper = mapper;
        }
    }
}