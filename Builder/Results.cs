internal record Result(int Id, object Data, TSolveType SolveType, bool IsTest, long ElapsedMilliseconds = 0) {
    public override string ToString() => $"Day {Id} ({SolveType}{(IsTest ? " TEST " : "")}) -> {Data} ({ElapsedMilliseconds/1000.0:0.000} s)";
}
