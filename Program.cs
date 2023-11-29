//var daysBuilder = new DaysBuilder(basePath: @"C:\Users\simonep\source\repos\AoC\Aoc\Input\");
var daysBuilder = new DaysBuilder(basePath: @"/home/simone/Scrivania/AdventOfCode/2023/");

daysBuilder
    .AddDay(1, new Day01())
    .SetTests(1, 2)
    .Solve();
