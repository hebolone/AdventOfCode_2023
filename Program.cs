﻿var daysBuilder = new DaysBuilder(basePath: @"C:\Users\simonep\source\repos\AoC\Aoc\Input\");

daysBuilder
    .AddDay(1, new Day01())
    .SetTests(1,2)
    .Solve();
