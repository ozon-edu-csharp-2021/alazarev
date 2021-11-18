using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class MerchPack : Entity, IAggregationRoot
    {
        private List<MerchItem> _positions;

        public MerchType Type { get; private set; }

        public IReadOnlyCollection<MerchItem> Positions => _positions.AsReadOnly();

        public MerchPack(MerchType type)
        {
            Type = type;
            _positions = new List<MerchItem>();
        }

        public MerchPack(int id, MerchType type, IEnumerable<MerchItem> merchItems)
        {
            Id = id;
            Type = type;
            _positions = new List<MerchItem>(merchItems);
        }


        public void AddPosition(MerchItem position)
        {
            _positions.Add(position);
        }

        public void RemovePosition(MerchItem position)
        {
            var toRemove = _positions.FirstOrDefault(p => Equals(p.Sku, position.Sku));
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