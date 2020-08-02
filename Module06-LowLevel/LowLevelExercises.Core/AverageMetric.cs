using System.Threading;

namespace LowLevelExercises.Core
{
    /// <summary>
    /// A simple class for reporting a specific value and obtaining an average.
    /// </summary>
    public class AverageMetric
    {
        private int _sum;
        private int _count;

        public void Report(int value)
        {
            Interlocked.Add(ref _sum, value);
            Interlocked.Increment(ref _count);
        }

        public double Average
        {
            get
            {
                var sum = Volatile.Read(ref _sum);
                var count = Volatile.Read(ref _count);
                return Calculate(count, sum);
            }
        }

        static double Calculate(in int count, in int sum)
        {
            // DO NOT change the way calculation is done.

            if (count == 0)
            {
                return double.NaN;
            }

            return (double) sum / count;
        }
    }
}