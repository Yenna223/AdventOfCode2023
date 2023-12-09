using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode2023.HauntedWastelandPuzzle;

namespace AdventOfCode2023
{
    public class MirageMaintenance
    {

        [Benchmark]
        public long SolveV1()
        {
            return GetImput()
                .AsParallel()
                .Select(x=> Calculate(GetNumbers(x)))
                .Sum();
        }


        [Benchmark]
        public long SolveV2()
        {
            
            return GetImput()
                .AsParallel()
                .Select(x => Calculate(GetNumbers(x).Reverse().ToArray()))
                .Sum();
        }

        public long Calculate(long[] array)
        {
            if (array.All(x => x == 0))
            {
                return 0;
            }

            var newArray = new long[array.Length - 1];

            for (var i = 0; i < newArray.Length; i++)
            {
                newArray[i] = array[i + 1] - array[i];
            }

            var newNumber = Calculate(newArray);

            return array.Last() + newNumber;
        }

        private long[] GetNumbers(string line)
        {
            return line.Split(" ").Select(x => Convert.ToInt64(x)).ToArray();
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input9.txt"));
        }
    }
}
