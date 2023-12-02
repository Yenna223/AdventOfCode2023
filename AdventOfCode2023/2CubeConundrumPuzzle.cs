using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    [MemoryDiagnoser(true)]
    public class CubeConundrumPuzzle
    {
        private int MaxNumberOfRedCubes = 12;
        private int MaxNumberOfBlueCubes = 14;
        private int MaxNumberOfGreenCubes = 13;

        private readonly int GamePrefixLength = "Game ".Length;

        private string BlueText = "blue";
        private string RedText = "red";
        private string GreenText = "green";


        [Benchmark]
        public int SolveV1()
        {
            return GetImput()
            .AsParallel()
            .Select(x => GetIdIfIsPossibleGame1(x))
            .Sum();
        }

        [Benchmark]
        public int SolveV2()
        {
            return GetImput()
            .AsParallel()
            .Select(x => GetIdIfIsPossibleGame2(x))
            .Sum();
        }

        private IEnumerable<string> GetImput()
        {
            return File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input2.txt"));
        }

        public int GetIdIfIsPossibleGame1(ReadOnlySpan<char> line)
        {
            var withoutGamePrefix = line.Slice(line.IndexOf(':') + 1);
            foreach (var game in GetGame(withoutGamePrefix.ToString()))
            {
                if (game.Red > MaxNumberOfRedCubes || game.Blue > MaxNumberOfBlueCubes || game.Green > MaxNumberOfGreenCubes)
                {
                    return 0;
                }
            }
            return Convert.ToInt32(line.Slice(GamePrefixLength, line.IndexOf(':') - GamePrefixLength).ToString());
        }


        private IEnumerable<(int Red, int Blue, int Green)> GetGame(string line)
        {
            var games = line.Split(";");
            
            foreach (var game in games)
            {
                yield return GetNumberOfCubesInRound(game);
            }
        }

        public int GetIdIfIsPossibleGame2(ReadOnlySpan<char> line)
        {
            var withoutGamePrefix = line.Slice(line.IndexOf(':') + 1);
            return GetGame2(withoutGamePrefix.ToString());
        }

        private int GetGame2(string line)
        {
            var games = line.Split(";");

            int maxRed = 0;
            int maxBlue = 0;
            int maxGreen = 0;

            foreach (var game in games)
            {
                var (red, blue, green) = GetNumberOfCubesInRound(game);
                if (maxRed < red)
                {
                   maxRed = red;
                }

                if (maxBlue < blue)
                {
                    maxBlue = blue;
                }

                if (maxGreen < green)
                {
                    maxGreen = green;
                }
            }

            return maxRed * maxBlue  * maxGreen;
        }

        private (int Red, int Blue, int Green) GetNumberOfCubesInRound(string game)
        {
            var cubes = game.Split(",");
            int red = 0;
            int blue = 0;
            int green = 0;

            foreach (var cube in cubes)
            {
                if (cube.Contains(RedText))
                {
                    red += Convert.ToInt32(cube.Replace(RedText, ""));
                }
                if (cube.Contains(BlueText))
                {
                    blue += Convert.ToInt32(cube.Replace(BlueText, ""));
                }
                if (cube.Contains(GreenText))
                {
                    green += Convert.ToInt32(cube.Replace(GreenText, ""));
                }
            }
            return (red, blue, green);
        }


    }
}
