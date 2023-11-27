namespace Builder;

internal interface IDaysBuilder {
    
    IDaysBuilder AddDay(int id, Day day);
    
    IDaysBuilder Solve(int id = -1, TSOLVETYPE solveType = TSOLVETYPE.BOTH);

    IDaysBuilder SetTests(params int [] ids);
}