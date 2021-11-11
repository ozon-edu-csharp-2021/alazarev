using System;
using OzonEdu.MerchApi.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.EmployeeAggregate
{
    public sealed class Employee : Entity
    {
        public PersonName FullName { get; }
        public Email Email { get; }
        public ClothingSize ClothingSize { get; }
        public Height Height { get; }

        public Employee(Email email, PersonName fullName, ClothingSize clothingSize, Height height)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            ClothingSize = clothingSize;
            Height = height;
        }

        public Employee(int id, Email email, PersonName fullName, ClothingSize clothingSize, Height height) : this(
            email, fullName, clothingSize, height)
        {
            Id = id;
        }
    }
}