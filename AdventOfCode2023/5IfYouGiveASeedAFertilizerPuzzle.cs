using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    [MemoryDiagnoser(true)]
    public class IfYouGiveASeedAFertilizerPuzzle
    {
        [Benchmark]
        public long SolveV1()
        {
            var input = GetImput().ToList();
            long[] source = GetNumbers(input[0]);
            long?[] destination = new long?[source.Length];

            bool insideArray = true;
            for (int i = 3; i < input.Count; i++)
            {
                var result = TransformV1(input[i], source, destination);
                if (!result && insideArray)
                {
                    FillEmpty(source, destination);
                    Overwrite(source, destination);
                    insideArray = false;
                    continue;
                }
                if (result && !insideArray)
                {
                    insideArray = true;
                }
            }

            FillEmpty(source, destination);
            Overwrite(source, destination);
            return source.Min();
        }

        [Benchmark]
        public long SolveV2()
        {
            var input = GetImput().ToList();
            var ranges = GetRanges(GetNumbers(input[0]));

            bool insideArray = true;
            for (int i = 3; i < input.Count; i++)
            {
                var result = TransformV2(input[i], ranges);
                if (!result && insideArray)
                {
                    foreach (var r in ranges)
                    {
                        r.CheckedInThatRun = false;
                    }
                    insideArray = false;
                    continue;
                }
                if (result && !insideArray)
                {
                    insideArray = true;
                }
            }

            return ranges.Select(x=> x.Mini).Min();
        }

        private bool TransformV2(string line, List<Range> ranges)
        {
            var numbers = GetNumbers(line);
            if (!numbers.Any())
            {
                return false;
            }
            long range = numbers[2];

            long minSource = numbers[1];
            long maxSource = minSource + range - 1;

            long minDestination = numbers[0];

            var newRanges = new List<Range>();

            foreach (var r in ranges.Where(x => !x.CheckedInThatRun))
            {
                if (r.Mini >= minSource && r.Max <= maxSource)
                {
                    var y = r.Max - r.Mini;
                    var x = r.Mini - minSource;
                    r.Mini = minDestination + x;
                    r.Max = r.Mini + y;
                    r.CheckedInThatRun = true;
                    continue;
                }

                if (r.Mini < minSource && r.Max >= minSource && r.Max <= maxSource)
                {
                    var oldValue = r.Mini;
                    var y = r.Max - minSource;
                    r.Mini = minDestination;
                    r.Max = r.Mini + y;
                    r.CheckedInThatRun = true;
                    newRanges.Add(new Range() { Mini = oldValue, Max = minSource - 1, CheckedInThatRun = false});
                    continue;
                }

                if (r.Mini >= minSource && r.Mini <= maxSource && r.Max > maxSource)
                {
                    var oldValue = r.Max;
                    var y = maxSource - minSource;
                    var x = r.Mini - minSource;
                    r.Mini = minDestination + x;
                    r.Max = minDestination + y;
                    r.CheckedInThatRun = true;
                    newRanges.Add(new Range() { Mini = maxSource + 1, Max = oldValue, CheckedInThatRun = false });
                    continue;
                }

                if (r.Mini < minSource && maxSource < r.Max)
                {
                    newRanges.Add(new Range() { Mini = r.Mini, Max = minSource - 1, CheckedInThatRun = false });
                    newRanges.Add(new Range() { Mini = maxSource + 1, Max = r.Max, CheckedInThatRun = false });
                    var y = maxSource - minSource;
                    r.Mini = minDestination;
                    r.Max = minDestination + y;
                    r.CheckedInThatRun = true;

                    continue;
                }

            }

            ranges.AddRange(newRanges);

            return true;
        }



        private bool TransformV1(string line, long[] source, long?[] destination)
        {
            var numbers = GetNumbers(line);
            if (!numbers.Any())
            {
                return false;
            }
            long range = numbers[2];

            long minSource = numbers[1];
            long maxSource = minSource + range - 1;

            long minDestination = numbers[0];

            for (int j = 0; j < source.Length; j++)
            {
                if (source[j] >= minSource && source[j] <= maxSource)
                {
                    var x = source[j] - minSource;
                    destination[j] = minDestination + x;
                }
            }
            return true;
        }

        private void Overwrite(long[] source, long?[] destination)
        {
            for (int j = 0; j < source.Length; j++)
            {
                source[j] = destination[j].Value;
                destination[j] = null;

            }
        }

        private void FillEmpty(long[] source, long?[] destination)
        {
            for (int j = 0; j < source.Length; j++)
            {
                if (destination[j] == null)
                {
                    destination[j] = source[j];
                }
            }
        }

        private long[] GetNumbers(string line)
        {
            return Regex.Matches(line.ToString(), @"\d+").OfType<Match>().Select(m => Convert.ToInt64(m.Value)).ToArray();
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input5.txt"));
        }

        private List<Range> GetRanges(long[] number)
        {
            var ranges = new List<Range>();
            for (int i = 0; i < number.Length; i+=2)
            {
                ranges.Add(new Range() { Mini = number[i], Max = number[i] + number[i + 1] - 1, CheckedInThatRun = false });
            }
            return ranges;
        }


        private class Range
        {
            public long Mini { get; set; }
            public long Max { get; set; }
            public bool CheckedInThatRun { get; set; }
        }
    }
}
