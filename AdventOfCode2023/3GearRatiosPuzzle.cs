using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2023
{
    [MemoryDiagnoser(true)]
    public class GearRatiosPuzzle
    {
        private char[][] EngineSchema;
        private readonly char[] Numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };


        [Benchmark]
        public int SolveV1()
        {
            FillEngineSchema();
            var sum = 0;
            for (int i = 0; i < EngineSchema.Length; i++)
            {
                sum += GetLevelSumV1(i);
            }
            return sum;
        }


        [Benchmark]
        public int SolveV2()
        {
            FillEngineSchema();
            var sum = 0;
            for (int i = 0; i < EngineSchema.Length; i++)
            {
                sum += GetLevelSumV2(i);
            }
            return sum;
        }

        private void FillEngineSchema()
        {
            var input = GetImput();
            EngineSchema = new char[input.ToList().Count][];
            var i = 0;
            foreach (var line in GetImput()) 
            {
                EngineSchema[i] = line.ToCharArray();
                i++;
            }
        }

        private int GetLevelSumV1(int level)
        {
            var sum = 0;
            var line = (ReadOnlySpan<char>)EngineSchema[level]!;

            var firstIndex = 0;
            bool insideNumber = false;

            for (var i = 0; i < line.Length; i++) 
            {
                if (Numbers.Contains(line[i]) && !insideNumber)
                {
                    firstIndex = i;
                    insideNumber = true;
                }

                if (!Numbers.Contains(line[i]) && insideNumber)
                {
                    var lastIndex = i - 1;
                    insideNumber = false;

                    if (IsLineNumberHasOnlyDotsOrNumbersAsNeighbors(level, firstIndex, lastIndex, line))
                    {
                        sum += 0;
                    }
                    else
                    {
                        sum += Convert.ToInt32(line.Slice(firstIndex, lastIndex - firstIndex + 1).ToString());
                    }
                }
            }

            if (Numbers.Contains(line[line.Length - 1]) && insideNumber)
            {
                var lastIndex = line.Length;

                if (IsLineNumberHasOnlyDotsOrNumbersAsNeighbors(level, firstIndex, lastIndex, line))
                {
                    sum += 0;
                }
                else
                {
                    sum += Convert.ToInt32(line.Slice(firstIndex).ToString());
                }
            }

            return sum;
        }

        private int GetLevelSumV2(int level)
        {
            var sum = 0;
            var line = (ReadOnlySpan<char>)EngineSchema[level]!;

            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '*')
                {
                    var result = new List<int>
                    {
                        HuntNumberLeft(i - 1, line),
                        HuntNumberRigth(i + 1, line)
                    };

                    if (level - 1 >= 0)
                    {
                        var upLine = (ReadOnlySpan<char>)EngineSchema[level - 1]!;
                        var(x, y) = HuntNumber(i, upLine);
                        result.Add(x);
                        result.Add(y);
                    }

                    if (level + 1 < EngineSchema.Length)
                    {
                        var downLine = (ReadOnlySpan<char>)EngineSchema[level + 1]!;
                        var (x, y) = HuntNumber(i, downLine);
                        result.Add(x);
                        result.Add(y);
                    }
                    var withoutZero = result.Where(x => x != 0);
                    if (withoutZero.Count() == 2)
                    {
                        sum += withoutZero.Aggregate(1, (x, y) => x * y);
                    }
                }
            }

            return sum;
        }

        private int HuntNumberLeft(int index, ReadOnlySpan<char> line)
        {
            var leftIndex = HuntLeftIndex(index, line);
            if (leftIndex == -1)
            {
                return 0;
            }

            if (index - leftIndex != 0)
            {
                return Convert.ToInt32(line.Slice(leftIndex, index - leftIndex + 1).ToString());
            }

            return Convert.ToInt32(line.Slice(index, 1).ToString());
        }

        private int HuntNumberRigth(int index, ReadOnlySpan<char> line)
        {
            var rigthIndex = HuntIndexRigth(index, line);
            if(rigthIndex == -1) 
            {
                return 0;
            }

            if (rigthIndex - index != 0)
            {
                return Convert.ToInt32(line.Slice(index, rigthIndex - index + 1).ToString());
            }

            return Convert.ToInt32(line.Slice(index, 1).ToString());
        }

 
        private (int, int) HuntNumber(int index, ReadOnlySpan<char> line)
        {
            if (index < 0)
            {
                index = 0;
            }

            var number = GetNumber(index, line);
            if (number != 0)
            {
                return (number, 0);
            }

            return (GetNumber(index - 1, line), GetNumber(index + 1, line));
        }

        private int GetNumber(int index, ReadOnlySpan<char> line)
        {
            var leftIndex = HuntLeftIndex(index, line);
            var rigtIndex = HuntIndexRigth(index, line);
            if (leftIndex != -1 && rigtIndex != -1)
            {
                return Convert.ToInt32(line.Slice(leftIndex, rigtIndex - leftIndex + 1).ToString());
            }
            return 0;
        }

        private int HuntLeftIndex(int index, ReadOnlySpan<char> line)
        {
            if (index < 0 || index >= line.Length)
            {
                return -1;
            }

            if (!Numbers.Contains(line[index]))
            {
                return -1;
            }

            var leftIndex = index;
            while (Numbers.Contains(line[leftIndex]))
            {
                if (leftIndex == 0)
                {
                    return leftIndex;
                }
                leftIndex--;
            }

            return leftIndex + 1;
        }

        private int HuntIndexRigth(int index, ReadOnlySpan<char> line)
        {
            if (index < 0 || index >= line.Length)
            {
                return -1;
            }

            if (!Numbers.Contains(line[index]))
            {
                return -1;
            }

            var rigthIndex = index;

            while (Numbers.Contains(line[rigthIndex]))
            {
                rigthIndex++;
                if (rigthIndex >= line.Length)
                {
                    return rigthIndex - 1;
                }
            }

            return rigthIndex - 1;
        }


        private bool IsLineNumberHasOnlyDotsOrNumbersAsNeighbors(int level, int firstIndex, int lastIndex, ReadOnlySpan<char> line)
        {
            if (!IsDotOrNumber(firstIndex - 1, line))
            {
                return false;
            }

            if (!IsDotOrNumber(lastIndex + 1, line))
            {
                return false;
            }

            if (level - 1 >= 0)
            {
                var upLine = (ReadOnlySpan<char>)EngineSchema[level - 1]!;
                if (!IsLineContainsOnlyDotsOrNumbers(firstIndex, lastIndex, upLine))
                {
                    return false;
                }
            }
 
            if (level + 1 < EngineSchema.Length)
            {
                var downLine = (ReadOnlySpan<char>)EngineSchema[level + 1]!;
                if (!IsLineContainsOnlyDotsOrNumbers(firstIndex, lastIndex, downLine))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsLineContainsOnlyDotsOrNumbers(int firstIndex, int lastIndex, ReadOnlySpan<char> line)
        {
            bool onlyDots = true;
            for (var i = firstIndex - 1; i <= lastIndex + 1; i++)
            {
                onlyDots = onlyDots && IsDotOrNumber(i, line);
            };
            return onlyDots;
        }

        private bool IsDotOrNumber(int index, ReadOnlySpan<char> line)
        {
            if (index < 0 || index >= line.Length)
            {
                return true;
            }
            return line[index] == '.' || Numbers.Contains(line[index]);
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input3.txt"));
        }
    }
}
