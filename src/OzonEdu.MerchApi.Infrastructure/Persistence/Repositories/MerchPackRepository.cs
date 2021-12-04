using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Persistence.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;
using OzonEdu.StockApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMapper _mapper;
        private const int Timeout = 5;

        public async Task<MerchPack> GetByMerchTypeAsync(MerchType merchType,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT id, type, positions from merch_pack where type=@Type;";

            var parameters = new
            {
                Type = (int)merchType
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchPackDtos = await connection.QueryAsync<MerchPackDto>(commandDefinition);

            var dto = merchPackDtos.FirstOrDefault();
            return _mapper.Map<MerchPack>(dto);
        }

        public async Task<MerchPack> Create(MerchPack merchPack, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO 
                    merch_pack (type,
                                positions)
                VALUES (@Type, 
                        (CAST(@Positions AS json)));";

            var parameters = new
            {
                Type = (int)merchPack.Type,
                Positions = JsonSerializer.Serialize(merchPack.Positions)
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(merchPack);
            return merchPack;
        }

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker, IMapper mapper)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _mapper = mapper;
        }
    }
}