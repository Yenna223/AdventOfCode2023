using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class WaitForItPuzzle
    {

        [Benchmark]
        public int SolveV1()
        {
            var input = GetImput().ToArray();
            var time = GetNumbers(input[0]);
            var distance = GetNumbers(input[1]);

            var result = 1;
            for (var i = 0; i < time.Length; i++)
            {
                var counter = 0;
                for (var j = 1; j < time[i]; j++)
                {
                    var d = (time[i] - j) * j;
                    if (d > distance[i])
                    {
                        counter++;
                    }
                }
                result *= counter;
            }

            return result;
        }

        [Benchmark]
        public long SolveV2()
        {
            var input = GetImput().ToArray();
            var time = GetNumbers(input[0].Replace(" ", ""))[0];
            var distance = GetNumbers(input[1].Replace(" ", ""))[0];

            var result = 0;
 
            for (var j = 1; j < time; j++)
            {
                var d = (time - j) * j;
                if (d > distance)
                {
                    result++;
                }
            }

            return result;
        }


        private long[] GetNumbers(string line)
        {
            return Regex.Matches(line.ToString(), @"\d+").OfType<Match>().Select(m => Convert.ToInt64(m.Value)).ToArray();
        }


        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input6.txt"));
        }
    }
}
