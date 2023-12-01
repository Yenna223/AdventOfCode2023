namespace AdventOfCode2023
{
    internal class TrebuchetPuzzle
    {
        private readonly char[] Numbers = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        private static Dictionary<string, int> AllNumbers = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
            ["1"] = 1,
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9
        };

        public int SolveV1(IEnumerable<string> input)
        {
            return input
            .AsParallel()
            .Select(x => GetNumberV1(x))
            .Sum();
        }

        public int SolveV2(IEnumerable<string> input)
        {
            return input
            .AsParallel()
            .Select(x => GetNumberV2(x))
            .Sum();
        }

        private int GetNumberV1(ReadOnlySpan<char> line)
        {
            var firstIndex = line.IndexOfAny(Numbers);
            var lastIndex = line.LastIndexOfAny(Numbers);

            return (line[firstIndex] - '0') * 10 + (line[lastIndex] - '0');
        }

        private int GetNumberV2(ReadOnlySpan<char> line)
        {
            var firstIndex = int.MaxValue;
            var lastIndex = int.MinValue;
            var firstNumber = 0;
            var lastNumber = 0;

            foreach (var numberKey in AllNumbers.Keys)
            {
                var fIndex = line.IndexOf(numberKey);
                var lIndex2 = line.LastIndexOf(numberKey);

                if (fIndex < 0)
                {
                    continue;
                }
                if (fIndex < firstIndex)
                {
                    firstIndex = fIndex;
                    firstNumber = AllNumbers[numberKey];
                }

                if (lIndex2 > lastIndex)
                {
                    lastIndex = lIndex2;
                    lastNumber = AllNumbers[numberKey];
                }
            }

            return firstNumber * 10 + lastNumber;
        }
    }
}
