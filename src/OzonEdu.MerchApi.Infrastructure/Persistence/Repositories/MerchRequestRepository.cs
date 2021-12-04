using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Npgsql;
using OpenTracing;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Infrastructure.Persistence.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;
using OzonEdu.StockApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class MerchRequestRepository : IMerchRequestRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMapper _mapper;
        private readonly ITracer _tracer;
        private const int Timeout = 5;

        public async Task<IEnumerable<MerchRequest>> GetAllEmployeeRequestsAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default)
        {
            using var span = _tracer.BuildSpan($"{nameof(MerchRequestRepository)}.GetAllEmployeeRequestsAsync")
                .StartActive();
            
            const string sql = @"
SELECT id, 
       started_at, 
       manager_email, 
       employee_id,
       status, 
       requested_merch_type, 
       mode, 
       reserved_at,
       request_merch 
from merch_request 
where employee_id=@EmployeeId;";

            var parameters = new
            {
                EmployeeId = employeeId.Value
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

        public async Task<IEnumerable<MerchRequest>> GetAllWaitingForSupplyRequestsAsync(
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT id, 
       started_at, 
       manager_email, 
       employee_id, 
       status, 
       requested_merch_type, 
       mode, 
       reserved_at,
       request_merch 
from merch_request 
where status=@Status
order by started_at;";

            var parameters = new
            {
                Status = MerchRequestStatus.WaitingForSupply.Id
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
                INSERT INTO 
                    merch_request (started_at, 
                                   manager_email, 
                                   employee_id, 
                                   status, 
                                   requested_merch_type,
                                   mode,
                                   reserved_at, 
                                   request_merch)
                VALUES (@StartAt, 
                        @ManagerEmail, 
                        @EmployeeId, 
                        @Status, 
                        @RequestedMerchType, 
                        @Mode, 
                        @ReservedAt, 
                        (CAST(@RequestMerch AS json)));";

            var parameters = new
            {
                StartAt = request.StartedAt,
                ManagerEmail = request.ManagerEmail.Value,
                EmployeeId = request.EmployeeId.Value,
                Status = request.Status.Id,
                RequestedMerchType = (int)request.RequestedMerchType,
                Mode = request.Mode.Id,
                ReservedAt = request.ReservedAt,
                RequestMerch = JsonSerializer.Serialize(request.Items)
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

        public async Task<MerchRequest> UpdateAsync(MerchRequest request, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE merch_request 
                SET (started_at, 
                    manager_email, 
                    employee_id, 
                    status, 
                    requested_merch_type,
                    mode,
                    reserved_at, 
                    request_merch)
                = (@StartAt, 
                   @ManagerEmail, 
                   @EmployeeId,
                   @Status,
                   @RequestedMerchType,
                   @Mode,
                   @ReservedAt,
                   (CAST(@RequestMerch AS json)))
                WHERE id=@Id;";

            var parameters = new
            {
                Id = request.Id,
                StartAt = request.StartedAt,
                ManagerEmail = request.ManagerEmail.Value,
                EmployeeId = request.EmployeeId.Value,
                Status = request.Status.Id,
                RequestedMerchType = (int)request.RequestedMerchType,
                Mode = request.Mode.Id,
                ReservedAt = request.ReservedAt,
                RequestMerch = JsonSerializer.Serialize(request.Items)
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
            IChangeTracker changeTracker, IMapper mapper, ITracer tracer)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _mapper = mapper;
            _tracer = tracer;
        }
    }
}