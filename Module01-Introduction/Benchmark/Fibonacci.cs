using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [NativeMemoryProfiler]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            var memoizator = new ulong[n];
            return Recursive(n);

            ulong Recursive(ulong i)
            {
                if (i == 1 || i == 2)
                    return 1;

                var memoizatorIndex = i - 1;
                if (memoizator[memoizatorIndex] != 0)
                    return memoizator[memoizatorIndex];

                memoizator[memoizatorIndex] = Recursive(i - 1) + Recursive(i - 2);
                return memoizator[memoizatorIndex];
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            var twoStepsBefore = 0uL;
            var stepBefore = 1uL;
            
            for (var i = 2uL; i < n; i++)
            {
                var tmp = stepBefore + twoStepsBefore;
                twoStepsBefore = stepBefore;
                stepBefore = tmp;
            }

            return stepBefore + twoStepsBefore;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}
