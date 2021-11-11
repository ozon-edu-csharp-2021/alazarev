using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchApi.Domain.AggregationModels.HumanResourceManagerAggregate;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class MerchPack : Entity, IAggregationRoot, IEnumerable<MerchItem>
    {
        private List<MerchItem> _positions;

        /// <summary>
        /// Ответственный за мерч менеджер
        ///
        /// Считаем, что за каждым паком числится ответственный менеджер
        /// (возможно менеджер должен быть закреплен за сотрудником)
        /// </summary>
        public HumanResourceManagerId HumanResourceManagerId { get; private set; }

        public MerchType Type { get; private set; }

        public MerchPack(MerchType type, HumanResourceManagerId humanResourceManagerId)
        {
            Type = type;
            HumanResourceManagerId = humanResourceManagerId;
            _positions = new List<MerchItem>();
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

        public IEnumerator<MerchItem> GetEnumerator() => _positions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _positions.GetEnumerator();
    }
}