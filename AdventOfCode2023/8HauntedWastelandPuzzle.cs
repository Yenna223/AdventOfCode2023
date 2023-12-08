using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2023.HauntedWastelandPuzzle;

namespace AdventOfCode2023
{
    public class HauntedWastelandPuzzle
    {
        [Benchmark]
        public int SolveV1()
        {
            var input = GetImput().ToList();
            var instruction = input[0];
            input.RemoveAt(0);
            input.RemoveAt(0);

            var roads = input.AsParallel().Select(x=> new Road(x)).ToList();

            var numberOfSteps = 0;
            var current = roads.First(x => x.Name == "AAA");
            while (current.Name != "ZZZ") 
            {
                for (int i = 0; i < instruction.Length; i++)
                {
                    if (instruction[i] == 'L')
                    {
                        current = roads.First(x => x.Name == current.Left);
                    }
                    if (instruction[i] == 'R')
                    {
                        current = roads.First(x => x.Name == current.Rigth);
                    }

                    numberOfSteps++;
                    if (current.Name == "ZZZ")
                    {
                        break;
                    }
                }
            }

            return numberOfSteps;
        }


        [Benchmark]
        public long SolveV2()
        {
            var input = GetImput().ToList();
            var instruction = input[0];
            input.RemoveAt(0);
            input.RemoveAt(0);

            var roads = input.AsParallel().Select(x => new Road(x)).ToList();

            var numbersOfSteps = new List<long>();

            foreach (var road in roads.Where(x => x.IsEndWithA))
            {
                numbersOfSteps.Add(CountSteps(roads, instruction, road.Name));
            }

            return FindLeastCommonMultiple(numbersOfSteps);
        }

        private long CountSteps(List<Road> roads, string instruction, string currentName)
        {
            var numberOfSteps = 0;
            var current = roads.First(x => x.Name == currentName);
            while (!current.IsEndWithZ)
            {
                for (int i = 0; i < instruction.Length; i++)
                {
                    if (instruction[i] == 'L')
                    {
                        current = roads.First(x => x.Name == current.Left);
                    }
                    if (instruction[i] == 'R')
                    {
                        current = roads.First(x => x.Name == current.Rigth);
                    }

                    numberOfSteps++;
                    if (current.IsEndWithZ)
                    {
                        break;
                    }
                }
            }

            return numberOfSteps;
        }

        private long FindLeastCommonMultiple(List<long> numbers) => numbers.Aggregate(1L, (current, number) => current / GreatestCommonDivisor(current, number) * number);

        private long GreatestCommonDivisor(long number1, long number2)
        {
            while (number2 != 0)
            {
                number1 %= number2;
                (number1, number2) = (number2, number1);
            }

            return number1;
        }

        public class Road
        {
            public Road(ReadOnlySpan<char> value)
            {
                Name = value.Slice(0, 3).ToString();
                var index = value.IndexOf(',');
                Left = value.Slice(7, 3).ToString();
                Rigth = value.Slice(index + 2, 3).ToString();
                IsEndWithZ = Name[2] == 'Z';
                IsEndWithA = Name[2] == 'A';
            }
            public string Name { get; private set; }
            public string Left { get; private set; }
            public string Rigth { get; private set; }

            public bool IsEndWithZ { get; private set; }
            public bool IsEndWithA { get; private set; }
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input8.txt"));
        }
    }
}
