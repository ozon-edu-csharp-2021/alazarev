using FluentMigrator;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(2)]
    public class EmployeeTableSeedData:Migration {
        public override void Up()
        {
            Insert.IntoTable("employee").Row(new 
            {
                id = 10001,
                email = "aleksey.lazarev@hotmail.com",
                first_name = "Aleksey",
                last_name = "Lazarev"
            });
            
            Insert.IntoTable("employee").Row(new 
            {
                id = 10002,
                email = "lazarev86@gmail.com",
                first_name = "Vasilii",
                last_name = "Vasilev"
            });
        }

        public override void Down()
        {
            Delete.FromTable("employee").Row(new { id = 10001 });
            Delete.FromTable("employee").Row(new { id = 10002 });
        }
    }
}