namespace Builder;

internal interface IDaysBuilder {
    
    IDaysBuilder AddDay(int id, Day day);
    
    IDaysBuilder Solve(int? id = null, TSolveType solveType = TSolveType.BOTH);

    IDaysBuilder SetTests(params int [] ids);

}