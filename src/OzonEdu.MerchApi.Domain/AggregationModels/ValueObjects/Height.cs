using System;

namespace OzonEdu.MerchApi.Domain.AggregationModels.ValueObjects
{
    public class Height
    {
        public double Centimeters { get; }

        public Height(double centimeters)
        {
            if (centimeters <= 0 || centimeters > 250)
            {
                throw new Exception("Incorrect height");
            }
            Centimeters = centimeters;
        }

        public static Height FromMetrics(double centimeters)
        {
            return new(centimeters);
        }

        public static Height FromImperial(int feet, int inches)
        {
            return new((feet * 12 + inches) * 2.54);
        }
    }
}