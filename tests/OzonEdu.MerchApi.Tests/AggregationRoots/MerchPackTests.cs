using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchRequestAggregate;
using OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchApi.Tests.AggregationRoots
{
    public class MerchPackTests
    {
        [Fact]
        public void MerchPack_AddThreePositions()
        {
            var pack = new MerchPack(MerchType.WelcomePack);
            pack.AddPosition(new MerchItem(new Sku(1), new Name("Bag"), MerchCategory.Bag));
            pack.AddPosition(new MerchItem(new Sku(2), new Name("Notepad"), MerchCategory.Notepad));
            pack.AddPosition(new MerchItem(new Sku(3), new Name("Socks"), MerchCategory.Socks));

            Assert.Equal(3, pack.Positions.Count());
        }

        [Fact]
        public void MerchPack_ClearAll()
        {
            var pack = new MerchPack(MerchType.WelcomePack);
            pack.AddPosition(new MerchItem(new Sku(1), new Name("Bag"), MerchCategory.Bag));
            pack.AddPosition(new MerchItem(new Sku(2), new Name("Notepad"), MerchCategory.Notepad));
            pack.AddPosition(new MerchItem(new Sku(3), new Name("Socks"), MerchCategory.Socks));
            pack.ClearAllPositions();
            Assert.Empty(pack.Positions);
        }

        [Fact]
        public void MerchPack_RemoveNotepad()
        {
            var pack = new MerchPack(MerchType.WelcomePack);
            pack.AddPosition(new MerchItem(new Sku(1), new Name("Bag"), MerchCategory.Bag));
            pack.AddPosition(new MerchItem(new Sku(2), new Name("Notepad"), MerchCategory.Notepad));
            pack.AddPosition(new MerchItem(new Sku(3), new Name("Socks"), MerchCategory.Socks));
            pack.RemovePosition(new MerchItem(new Sku(2), new Name("Notepad"), MerchCategory.Notepad));
            Assert.Equal(2, pack.Positions.Count());
        }
    }
}