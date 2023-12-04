using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2023
{
    [MemoryDiagnoser(true)]
    public class ScratchcardsPuzzle
    {

        [Benchmark]
        public int SolveV1()
        {
            return GetImput()
            .AsParallel()
            .Select(x => GetValueV1(x))
            .Sum();
        }


        [Benchmark]
        public int SolveV2()
        {

            var input = GetImput();
            Dictionary<int, int> cards = new Dictionary<int, int>();
            foreach (var card in input)
            {
                (int cardNumber, int numberOfWinnigCards) = AnalyseLine(card);
                if (!cards.ContainsKey(cardNumber))
                {
                    cards.Add(cardNumber, 1);
                }
                else
                {
                    cards[cardNumber]++;
                }

                for (int i = 1; i <= numberOfWinnigCards; i++)
                {
                    var key = cardNumber + i;
                    var numberOfCardsToAdd = cards[cardNumber];
                    if (cards.ContainsKey(key))
                    {
                        cards[key]+= numberOfCardsToAdd;
                    }
                    else
                    {
                        cards.Add(key, numberOfCardsToAdd);
                    }
                }  
            }
            
            return cards.Select(x=> x.Value).Sum();
        }


        private int GetValueV1(ReadOnlySpan<char> line)
        {
            return CalculatePoints(GetWinningCards(line));
        }


        private (int cardNumber, int numberOfWinnigCards) AnalyseLine(ReadOnlySpan<char> line)
        {
            var numberOfCard = Convert.ToInt32(Regex.Match(line.ToString(), @"\d+").Value);
            var numberOfWinnigCards = GetWinningCards(line);
            return (numberOfCard, numberOfWinnigCards);
        }

        private int GetWinningCards(ReadOnlySpan<char> line)
        {
            var indexOfColon = line.IndexOf(':') + 1;
            line = line.Slice(indexOfColon);
            var indexOfLine = line.IndexOf('|');

            var winingNumbers = ConvertToList(line.Slice(0, indexOfLine));
            var numbers = ConvertToList(line.Slice(indexOfLine + 1));

            return numbers.Intersect(winingNumbers).Count();
        }

        private int CalculatePoints(int numberOfElements)
        {
            if (numberOfElements == 0)
            {
                return numberOfElements;
            }

            var result = 1;
            for (var i = result; i < numberOfElements; i++)
            {
                result = result * 2;
            }

            return result;
        }

        private List<int> ConvertToList(ReadOnlySpan<char> line)
        {
            return Regex.Matches(line.ToString(), @"\d+").OfType<Match>().Select(m => Convert.ToInt32(m.Value)).ToList();
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input4.txt"));
        }
    }
}
