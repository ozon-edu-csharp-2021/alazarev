using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class MerchPack : Entity, IAggregationRoot
    {
        private List<MerchPackItem> _positions;

        public MerchType Type { get; private set; }

        public IReadOnlyCollection<MerchPackItem> Positions => _positions.AsReadOnly();

        public MerchPack(MerchType type)
        {
            Type = type;
            _positions = new List<MerchPackItem>();
        }

        public MerchPack(int id, MerchType type, IEnumerable<MerchPackItem> merchItems)
        {
            Id = id;
            Type = type;
            _positions = new List<MerchPackItem>(merchItems);
        }


        public void AddPosition(MerchPackItem position)
        {
            _positions.Add(position);
        }

        public void RemovePosition(MerchPackItem position)
        {
            var toRemove = _positions.FirstOrDefault(p => Equals(p.ItemId, position.ItemId));
            if (toRemove != null)
            {
                _positions.Remove(toRemove);
            }
        }

        public void ClearAllPositions()
        {
            _positions.Clear();
        }
    }
}