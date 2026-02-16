using System;

namespace NavigationSimulator.Helpers
{
    public static class NoiseGenerator
    {
        private static readonly Random _random = new Random();

        public static double NextGaussian(double mean, double standardDeviation)
        {
            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + standardDeviation * randStdNormal;
        }

        public static double NextDouble(double min, double max)
        {
            return _random.NextDouble() * (max - min) + min;
        }
    }
}
