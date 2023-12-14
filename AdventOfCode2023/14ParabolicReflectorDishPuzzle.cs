using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class ParabolicReflectorDishPuzzle
    {
        [Benchmark]
        public long SolveV1()
        {
            var input = GetImput().ToArray();
            var result = 0;
            for (var i = 0; i < input.Count(); i++)
            {
                for (var j = 0; j < input[i].Count(); j++)
                {
                    if (input[i][j] == '.')
                    {
                        for (int x = i + 1; x < input.Count(); x++)
                        {
                            if (input[x][j] == '#')
                            {
                                break;
                            }
                            if (input[x][j] == 'O')
                            {
                                input[x][j] = '.';
                                input[i][j] = 'O';
                                break;
                            }
                        }
                    }
                }
                result += input[i].Count(x => x == 'O') * (input.Length - i);
            }
            return result;
        }

        [Benchmark]
        public long SolveV2()
        {
            var input = GetImput().ToArray();
            var result = 0;

            for (var i = 0; i < 1000; i++)
            {
                RollNorth(input);
                RollWest(input);
                RollSouth(input);
                RollEast(input);
            }


            for (var i = 0; i < input.Count(); i++)
            {
                result += input[i].Count(x => x == 'O') * (input.Length - i);
            }

            return result;
        }


        private void RollNorth(char[][] input)
        {
            for (var i = 0; i < input.Count(); i++)
            {
                for (var j = 0; j < input[i].Count(); j++)
                {
                    if (input[i][j] == '.')
                    {
                        for (int x = i + 1; x < input.Count(); x++)
                        {
                            if (input[x][j] == '#')
                            {
                                break;
                            }
                            if (input[x][j] == 'O')
                            {
                                input[x][j] = '.';
                                input[i][j] = 'O';
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void RollSouth(char[][] input)
        {
            for (var i = input.Count() - 1; i >= 0; i--)
            {
                for (var j = 0; j < input[i].Count(); j++)
                {
                    if (input[i][j] == '.')
                    {
                        for (int x = i - 1; x >= 0; x--)
                        {
                            if (input[x][j] == '#')
                            {
                                break;
                            }
                            if (input[x][j] == 'O')
                            {
                                input[x][j] = '.';
                                input[i][j] = 'O';
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void RollWest(char[][] input)
        {
            for (var i = 0; i < input.Count(); i++)
            {
                for (var j = 0; j < input[i].Count(); j++)
                {
                    if (input[i][j] == '.')
                    {
                        for (int x = j + 1; x < input[i].Count(); x++)
                        {
                            if (input[i][x] == '#')
                            {
                                break;
                            }
                            if (input[i][x] == 'O')
                            {
                                input[i][x] = '.';
                                input[i][j] = 'O';
                                break;
                            }
                        }
                    }
                }
            }
        }


        private void RollEast(char[][] input)
        {
            for (var i = 0; i < input.Count(); i++)
            {
                for (var j = input[i].Count() - 1; j >= 0; j--)
                {
                    if (input[i][j] == '.')
                    {
                        for (int x = j - 1; x >= 0; x--)
                        {
                            if (input[i][x] == '#')
                            {
                                break;
                            }
                            if (input[i][x] == 'O')
                            {
                                input[i][x] = '.';
                                input[i][j] = 'O';
                                break;
                            }
                        }
                    }
                }
            }
        }


        private IEnumerable<char[]> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input14.txt")).Select(x=> x.ToArray());
        }
    }
}
