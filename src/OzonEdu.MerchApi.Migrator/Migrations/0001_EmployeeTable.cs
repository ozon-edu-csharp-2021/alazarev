using FluentMigrator;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(1)]
    public class EmployeeTable:Migration {
        public override void Up()
        {
            Create
                .Table("employee")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("first_name").AsString().Nullable()
                .WithColumn("last_name").AsString().Nullable()
                .WithColumn("email").AsString().NotNullable()
                .WithColumn("clothing_size").AsInt32().Nullable()
                .WithColumn("height").AsDouble().Nullable();
        }

        public override void Down()
        {
            Delete.Table("employee");
        }
    }
}