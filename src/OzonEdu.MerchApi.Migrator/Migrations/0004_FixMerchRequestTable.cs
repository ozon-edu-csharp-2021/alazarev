using FluentMigrator;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(4)]
    public class FixMerchRequestTable:Migration {
        public override void Up()
        {
            Delete.Column("employee_email").FromTable("merch_request");
            Delete.Column("clothing_size").FromTable("merch_request");
            Alter.Table("merch_request").AddColumn("employee_id").AsInt32().NotNullable()
                .Indexed("merch_request__employee_id_index");
        }

        public override void Down()
        {
            Delete.Column("employee_id").FromTable("merch_request");
            Alter.Table("merch_request").AddColumn("clothing_size").AsInt32().NotNullable();
            Alter.Table("merch_request").AddColumn("employee_email").AsString().NotNullable()
                .Indexed("merch_request__employee_email_index");
        }
    }
}