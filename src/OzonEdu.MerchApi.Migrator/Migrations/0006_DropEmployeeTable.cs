using FluentMigrator;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(6)]
    public class DropEmployeeTable:Migration {
        public override void Up()
        {
            Delete.Table("employee");
        }

        public override void Down()
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
    }
}