using BenchmarkDotNet.Attributes;

namespace AdventOfCode2023
{
    public class CamelCardsPuzzle
    {

        private static Dictionary<char, int> CardNumbers = new Dictionary<char, int>
        {
            ['A'] = 13,
            ['K'] = 12,
            ['Q'] = 11,
            ['J'] = 10,
            ['T'] = 9,
            ['9'] = 8,
            ['8'] = 7,
            ['7'] = 6,
            ['6'] = 5,
            ['5'] = 4,
            ['4'] = 3,
            ['3'] = 2,
            ['2'] = 1,
        };


        private static Dictionary<char, int> CardNumbers2 = new Dictionary<char, int>
        {
            ['A'] = 13,
            ['K'] = 12,
            ['Q'] = 11,
            ['J'] = 0,
            ['T'] = 9,
            ['9'] = 8,
            ['8'] = 7,
            ['7'] = 6,
            ['6'] = 5,
            ['5'] = 4,
            ['4'] = 3,
            ['3'] = 2,
            ['2'] = 1,
        };


        [Benchmark]
        public int SolveV1()
        {
            var hands = GetImput()
                .AsParallel()
                .Select(GetHandV1)
                .ToList();
            
            hands.Sort();
            var result = 0;

            for(int i = 0; i < hands.Count ; i++)
            {
                result += (hands.Count() - i) * hands[i].Bid;
            }
            return result;
        }

        [Benchmark]
        public int SolveV2()
        {
            var hands = GetImput()
                .AsParallel()
                .Select(GetHandV2)
                .ToList();

            hands.Sort();
            var result = 0;

            for (int i = 0; i < hands.Count; i++)
            {
                result += (hands.Count() - i) * hands[i].Bid;
            }
            return result;
        }

        private Hand GetHandV1(string line)
        {
            var splitLine = line.Split(' ');
            return new FirstHand(splitLine[0], Convert.ToInt32(splitLine[1]));
        }
        private Hand GetHandV2(string line)
        {
            var splitLine = line.Split(' ');
            return new SecondHand(splitLine[0], Convert.ToInt32(splitLine[1]));
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input7.txt"));
        }


        public abstract class Hand
        {
            public Hand(string value, int bid)
            {
                Value = value;
                Bid = bid;
                Type = SetType();
            }

            public string Value { get; private set; }
            public HandType Type { get; private set; }
            public int Bid { get; private set; }

            protected abstract HandType SetType();
        }

        public class FirstHand : Hand, IComparable
        {
            public FirstHand(string value, int bid) : base(value, bid)
            { 
            }

            public int CompareTo(object? obj)
            {
                var otherHand = obj as Hand;
                if(Type > otherHand.Type)
                {
                    return -1;
                }
                
                if (Type < otherHand.Type)
                {
                    return 1;
                }

                if (Type == otherHand.Type)
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        if (CardNumbers[Value[i]] == CardNumbers[otherHand.Value[i]])
                        {
                            continue;
                        }

                        return CardNumbers[Value[i]] < CardNumbers[otherHand.Value[i]] ? 1 : -1;
                    }
                }

                return 0;
            }

            protected override HandType SetType()
            {
                var groupByResult = Value.GroupBy(x => x);
                if (groupByResult.Any(x => x.Count() == 5))
                {
                    return HandType.FiveOfKind;
                }

                if (groupByResult.Any(x => x.Count() == 4))
                {
                    return HandType.FourOfKind;
                }

                if (groupByResult.Any(x => x.Count() == 3) && groupByResult.Any(x => x.Count() == 2))
                {
                    return HandType.FullHouse;
                }

                if (groupByResult.Any(x => x.Count() == 3))
                {
                    return HandType.ThreeOfKind;
                }

                if (groupByResult.Where(x => x.Count() == 2).Count() == 2)
                {
                    return HandType.TwoPair;
                }

                if (groupByResult.Where(x => x.Count() == 2).Count() == 1)
                {
                    return HandType.OnePair;
                }

                return HandType.HighCard;
            }


        }

        public class SecondHand : Hand, IComparable
        {
            public SecondHand(string value, int bid) : base(value, bid)
            {
            }

            public int CompareTo(object? obj)
            {
                var otherHand = obj as Hand;
                if (Type > otherHand.Type)
                {
                    return -1;
                }

                if (Type < otherHand.Type)
                {
                    return 1;
                }

                if (Type == otherHand.Type)
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        if (CardNumbers2[Value[i]] == CardNumbers2[otherHand.Value[i]])
                        {
                            continue;
                        }

                        return CardNumbers2[Value[i]] < CardNumbers2[otherHand.Value[i]] ? 1 : -1;
                    }
                }

                return 0;
            }

            protected override HandType SetType()
            {
                var numberOfJokers = Value.Where(x => x == 'J').Count();
                var groupByResult = Value.Replace("J", string.Empty).GroupBy(x => x);

                if (!groupByResult.Any())
                {
                    return HandType.FiveOfKind;
                }

                if (groupByResult.Any(x => x.Count() == 5 - numberOfJokers))
                {
                    return HandType.FiveOfKind;
                }

                if (groupByResult.Any(x => x.Count() == 4 - numberOfJokers))
                {
                    return HandType.FourOfKind;
                }

                var groupWithThree = groupByResult.Where(x => x.Count() == 3 - numberOfJokers).FirstOrDefault();
                if (groupWithThree != null)
                {
                    var groupByResult2 = Value.Replace("J", string.Empty).Replace(groupWithThree.Key.ToString(), string.Empty).GroupBy(x => x);
                    if ( groupByResult2.Any(x => x.Count() == 2))
                    {
                        return HandType.FullHouse;
                    }
                    else
                    {
                        return HandType.ThreeOfKind;
                    }
                }

                if (groupByResult.Where(x => x.Count() == 2 - numberOfJokers).Count() == 2)
                {
                    return HandType.TwoPair;
                }

                if (groupByResult.Where(x => x.Count() == 2 - numberOfJokers).Count() >= 1)
                {
                    return HandType.OnePair;
                }

                return HandType.HighCard;
            }


        }

        public enum HandType
        {
            FiveOfKind = 6,// where all five cards have the same label: AAAAA
            FourOfKind = 5,//where four cards have the same label and one card has a different label: AA8AA
            FullHouse = 4,// where three cards have the same label, and the remaining two cards share a different label: 23332
            ThreeOfKind = 3,//, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
            TwoPair = 2,// where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
            OnePair = 1,// where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
            HighCard = 0// where all cards' labels are distinct: 23456
        }
    }
}
