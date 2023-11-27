internal record Result(int id, object result, TSOLVETYPE solveType, bool isTest) {
    public override string ToString() => $"Day {id} ({solveType}{(isTest ? " TEST " : "")}) -> {result})";
}