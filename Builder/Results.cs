internal record Result(int Id, object Data, TSolveType SolveType, bool IsTest) {
    public override string ToString() => $"Day {Id} ({SolveType}{(IsTest ? " TEST " : "")}) -> {Data})";
}

internal record Result<T>(int Id, T Data, TSolveType SolveType, bool IsTest) {
    public override string ToString() => $"Day {Id} ({SolveType}{(IsTest ? " TEST " : "")}) -> {Data})";
}