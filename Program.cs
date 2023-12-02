//var inputFilesDir = @"C:\Users\simonep\source\repos\AoC\Aoc\Input\");
var inputFilesDir = @"/home/simone/Scrivania/AdventOfCode/2023/";
var daysBuilder = new DaysBuilder(basePath: inputFilesDir);

daysBuilder
    //.AddDay(1, new Day01())
    .AddDay(2, new Day02())
    //.SetTests(2)
    .Solve()
    ;
