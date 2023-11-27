internal record Result(int Id, object Data, TSOLVETYPE SolveType, bool IsTest) {
    public override string ToString() => $"Day {Id} ({SolveType}{(IsTest ? " TEST " : "")}) -> {Data})";
}
