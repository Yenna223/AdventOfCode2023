using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class CosmicExpansionPuzzle
    {

        [Benchmark]
        public long SolveV1()
        {
            return Calculate(2);
        }

        [Benchmark]
        public long SolveV2()
        {
            return Calculate(1000000);
        }

        private long Calculate(int n)
        {
            var input = GetImput();
            var galaxies = new List<Galaxy>();

            var y = 0;

            foreach (var line in input)
            {
                if (line.All(x => x == '.'))
                {
                    y += n;
                    continue;
                }

                var lineAsSpan = line.AsSpan();
                var galaxyIndex = Regex.Matches(line, "#").Cast<Match>().Select(m => m.Index).ToList();
                foreach (var index in galaxyIndex)
                {
                    galaxies.Add(new Galaxy { X = index, Y = y });
                }
                y++;
            }

            var rowsToAdd = Enumerable.Range(0, input.First().Length).Select(Convert.ToInt64).Except(galaxies.Select(x => x.X).ToList().Distinct());
            var diff = n - 1;
            foreach (var galaxy in galaxies)
            {
                var numberToAdd = rowsToAdd.Where(x => x < galaxy.X).Count();
                galaxy.X += numberToAdd * diff;
            }

            long result = 0;

            var checkedGalaxies = new List<Galaxy>();
            foreach (var galaxy in galaxies)
            {
                checkedGalaxies.Add(galaxy);
                foreach (var uncheckedGalaxy in galaxies.Except(checkedGalaxies))
                {
                    result += Math.Abs(uncheckedGalaxy.X - galaxy.X) + Math.Abs(uncheckedGalaxy.Y - galaxy.Y);
                }
            }

            return result;
        }

        private class Galaxy
        {
            public long X { get; set; }
            public long Y { get; set; }
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input11.txt"));
        }
    }
}
