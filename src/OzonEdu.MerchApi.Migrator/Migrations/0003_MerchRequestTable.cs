using FluentMigrator;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(3)]
    public class MerchRequestTable:Migration {
        public override void Up()
        {
            Create
                .Table("merch_request")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("started_at").AsDateTimeOffset().NotNullable()
                .WithColumn("manager_email").AsString().NotNullable()
                .WithColumn("employee_email").AsString().NotNullable().Indexed("merch_request__employee_email_index")
                .WithColumn("status").AsInt32().NotNullable().Indexed("merch_request__status_index")
                .WithColumn("clothing_size").AsInt32().NotNullable()
                .WithColumn("requested_merch_type").AsInt32().Nullable()
                .WithColumn("mode").AsInt32().NotNullable()
                .WithColumn("reserved_at").AsDateTimeOffset().Nullable()
                .WithColumn("request_merch").AsCustom("jsonb");
        }

        public override void Down()
        {
            Delete.Table("merch_request");
        }
    }
}