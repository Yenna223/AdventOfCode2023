using AdventOfCode2023;
using BenchmarkDotNet.Running;


//var summary = BenchmarkRunner.Run<IfYouGiveASeedAFertilizerPuzzle>();

var puzzle = new CosmicExpansionPuzzle();

Console.WriteLine(puzzle.SolveV1());
Console.WriteLine(puzzle.SolveV2());


Console.ReadLine();