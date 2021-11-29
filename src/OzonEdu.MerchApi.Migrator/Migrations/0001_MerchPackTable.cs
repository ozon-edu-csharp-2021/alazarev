using FluentMigrator;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(1)]
    public class MerchPackTable:Migration {
        public override void Up()
        {
            Create
                .Table("merch_pack")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("type").AsInt32().NotNullable()
                .WithColumn("positions").AsCustom("jsonb");
        }

        public override void Down()
        {
            Delete.Table("merch_pack");
        }
    }
}