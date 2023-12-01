using AdventOfCode2023;

var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputs", "input1.txt"));
var puzzle = new TrebuchetPuzzle();

Console.WriteLine(puzzle.SolveV1(lines));
Console.WriteLine(puzzle.SolveV2(lines));

