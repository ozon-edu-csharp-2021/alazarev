using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpCourse.Core.Lib.Enums;
using FluentMigrator;
using OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchApi.Infrastructure.Persistence.Models;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(4)]
    public class MerchPackTableSeedData:Migration {
        public override void Up()
        {
            var mp1 = new MerchPack(MerchType.WelcomePack);
            mp1.AddPosition(new MerchItem(new Sku(100), new Name("ostin"), MerchCategory.TShirt));
            mp1.AddPosition(new MerchItem(new Sku(200), new Name("adidas"), MerchCategory.Bag));
            
            var mp2 = new MerchPack(MerchType.VeteranPack);
            mp2.AddPosition(new MerchItem(new Sku(300), new Name("Notepad"), MerchCategory.Notepad));
            mp2.AddPosition(new MerchItem(new Sku(400), new Name("Socks"), MerchCategory.Socks));
            
            var mp3 = new MerchPack(MerchType.ProbationPeriodEndingPack);
            mp3.AddPosition(new MerchItem(new Sku(500), new Name("Sweatshirt"), MerchCategory.Sweatshirt));
            mp3.AddPosition(new MerchItem(new Sku(600), new Name("Bag"), MerchCategory.Bag));
            
            var jsonPositions1 = JsonSerializer.Serialize(mp1.Positions);
            var jsonPositions2 = JsonSerializer.Serialize(mp2.Positions);
            var jsonPositions3 = JsonSerializer.Serialize(mp3.Positions);
            
            Insert.IntoTable("merch_pack").Row(new 
            {
                id = 20001,
                type = (int)mp1.Type,
                positions = jsonPositions1
            });
            
            Insert.IntoTable("merch_pack").Row(new 
            {
                id = 20002,
                type = (int)mp2.Type,
                positions = jsonPositions2
            });
            
            Insert.IntoTable("merch_pack").Row(new 
            {
                id = 20003,
                type = (int)mp3.Type,
                positions = jsonPositions3
            });
        }

        public override void Down()
        {
            Delete.FromTable("merch_pack").Row(new { id = 20001 });
            Delete.FromTable("merch_pack").Row(new { id = 20002 });
            Delete.FromTable("merch_pack").Row(new { id = 20003 });
        }
    }
}