using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OzonEdu.MerchApi.Domain.Models
{
    public abstract class EnumerationWithDescription : Enumeration
    {
        public string Description { get; }

        protected EnumerationWithDescription(int id, string name, string description) : base(id, name)
        {
            Description = description;
        }
    }
}