using FluentMigrator;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(5)]
    public class MerchRequestTable:Migration {
        public override void Up()
        {
            Create
                .Table("merch_request")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("started_at").AsDateTimeOffset().NotNullable()
                .WithColumn("manager_email").AsString().NotNullable()
                .WithColumn("employee_id").AsInt32().NotNullable()
                .WithColumn("status").AsInt32().NotNullable()
                .WithColumn("requested_merch_type").AsInt32().Nullable()
                .WithColumn("mode").AsInt32().NotNullable()
                .WithColumn("reserved_at").AsDateTimeOffset().Nullable();
        }

        public override void Down()
        {
            Delete.Table("merch_request");
        }
    }
}