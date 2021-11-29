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
            pack.AddPosition(new MerchPackItem(new ItemId(1), new Name("Bag"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(2), new Name("Notepad"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(3), new Name("Socks"), new Quantity(1)));

            Assert.Equal(3, pack.Positions.Count());
        }

        [Fact]
        public void MerchPack_ClearAll()
        {
            var pack = new MerchPack(MerchType.WelcomePack);
            pack.AddPosition(new MerchPackItem(new ItemId(1), new Name("Bag"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(2), new Name("Notepad"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(3), new Name("Socks"), new Quantity(1)));
            pack.ClearAllPositions();
            Assert.Empty(pack.Positions);
        }

        [Fact]
        public void MerchPack_RemoveNotepad()
        {
            var pack = new MerchPack(MerchType.WelcomePack);
            pack.AddPosition(new MerchPackItem(new ItemId(1), new Name("Bag"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(2), new Name("Notepad"), new Quantity(1)));
            pack.AddPosition(new MerchPackItem(new ItemId(3), new Name("Socks"), new Quantity(1)));
            pack.RemovePosition(new MerchPackItem(new ItemId(2), new Name("Notepad"), new Quantity(1)));
            Assert.Equal(2, pack.Positions.Count());
        }
    }
}