using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Npgsql;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Domain.Contracts;
using OzonEdu.MerchApi.Infrastructure.Persistence.Interfaces;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;
using OzonEdu.StockApi.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace OzonEdu.MerchApi.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMapper _mapper;
        private const int Timeout = 5;

        public async Task<Employee> FindByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            const string sql = @"
SELECT id, first_name, last_name, email, clothing_size, height from employee where email=@Email;";

            var parameters = new
            {
                Email = email.Value
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var employeeDtos = await connection.QueryAsync<EmployeeDto>(commandDefinition);

            var dto = employeeDtos.FirstOrDefault();
            var employee = _mapper.Map<Employee>(dto);
            return employee;
        }

        public async Task<Employee> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO employee (first_name, last_name, email, clothing_size, height)
                VALUES (@FirstName, @LastName, @Email, @ClothingSize, @Height);";

            var parameters = new
            {
                FirstName = employee.FullName?.FirstName,
                LastName = employee.FullName?.LastName,
                Email = employee.Email.Value,
                ClothingSize = employee.ClothingSize?.Id,
                Height = employee.Height?.Centimeters
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(employee);
            return employee;
        }

        public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM Employee WHERE Id = @Id";
            var parameters = new
            {
                Id = employee.Id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(employee);
        }

        public async Task<Employee> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT id, first_name, last_name, email, clothing_size, height from employee 
                where e.Id='@Id'";

            var parameters = new
            {
                Id = id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var employeeDtos = await connection.QueryAsync<EmployeeDto>(commandDefinition);

            var dto = employeeDtos.FirstOrDefault();
            var employee = _mapper.Map<Employee>(dto);
            return employee;
        }

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker, IMapper mapper)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _mapper = mapper;
        }
    }
}