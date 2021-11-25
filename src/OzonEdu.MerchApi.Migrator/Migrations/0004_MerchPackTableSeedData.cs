using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpCourse.Core.Lib.Enums;
using FluentMigrator;
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
            mp1.AddPosition(new MerchPackItem(new ItemId(1), new Name("TShirtStarter XS"), new Quantity(1)));
            mp1.AddPosition(new MerchPackItem(new ItemId(7), new Name("NotepadStarter"), new Quantity(1)));
            
            var mp2 = new MerchPack(MerchType.VeteranPack);
            mp2.AddPosition(new MerchPackItem(new ItemId(13), new Name("TShirtVeteran"), new Quantity(1)));
            mp2.AddPosition(new MerchPackItem(new ItemId(14), new Name("NotepadVeteran"), new Quantity(1)));

            var mp3 = new MerchPack(MerchType.ProbationPeriodEndingPack);
            mp3.AddPosition(new MerchPackItem(new ItemId(5), new Name("TShirtAfterProbation"), new Quantity(1)));
            mp3.AddPosition(new MerchPackItem(new ItemId(6), new Name("SweatshirtAfterProbation"), new Quantity(1)));
            
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