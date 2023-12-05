//var inputFilesDir = @"C:\Users\simonep\source\repos\AoC\Aoc\Input\";
var inputFilesDir = @"/media/simone/datas/work/csharp/AdventOfCode_2023/Input";
var daysBuilder = new DaysBuilder(basePath: inputFilesDir);

daysBuilder
    //.AddDay(1, new Day01())
    //.AddDay(2, new Day02())
    //.AddDay(3, new Day03())
    //.AddDay(4, new Day04())
    .AddDay(5, new Day05())
    .SetTests()
    .Solve(solveType: TSolveType.ADVANCED)
    ;
