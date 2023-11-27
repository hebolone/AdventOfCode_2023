namespace Builder;

internal class DaysBuilder(string basePath) : IDaysBuilder {

    private readonly string _BasePath = basePath;
    private List<int> _TestDays = new();
    
    private Dictionary<int, Day> _Days = new();

    #region IDaysBuilder

    public IDaysBuilder AddDay(int id, Day day) {
        _Days.Add(id, day);
        return this;
    }

    public IDaysBuilder Solve(int id = -1, TSOLVETYPE solveType = TSOLVETYPE.BOTH) {

        var idToSolve = id == -1 ? _Days.Keys.Max() : id;

        var dayPresent = _Days.ContainsKey(idToSolve);
        if(dayPresent) {

            //  Read input file
            var solver = _Days[idToSolve];
            var isTest = _TestDays.Contains(idToSolve);
            solver.Input = ReadInput(idToSolve, isTest);

            //  Get solution
            var results = new List<Result>();
            switch(solveType) {
                case TSOLVETYPE.BASIC:
                    results.Add(new(idToSolve, solver.Basic(), solveType, isTest));
                    break;
                case TSOLVETYPE.ADVANCED:
                    results.Add(new(idToSolve, solver.Advanced(), solveType, isTest));
                    break;
                default:
                    results.Add(new(idToSolve, solver.Basic(), TSOLVETYPE.BASIC, isTest));
                    results.Add(new(idToSolve, solver.Advanced(), TSOLVETYPE.ADVANCED, isTest));
                    break;
            }

            results.ForEach(r => {
                Console.WriteLine(r.ToString());
            });
        } else {
            throw new ArgumentException($"No solution for day '{idToSolve}'");
        }

        return this;

    }

    public IDaysBuilder SetTests(params int [] ids) {
        foreach(var i in ids) {
            _TestDays.Add(i);
        }

        return this;
    }

    #endregion

    private List<string> ReadInput(int id, bool isTest) {
        var retValue = new List<string>();
        var lines = File.ReadLines(GetInputFileName(id, isTest));
        
        foreach(var line in lines) {
            retValue.Add(line);
        }

       return retValue;
    }

    private string GetInputFileName(int id, bool isTest) => Path.Combine(_BasePath, $"day_{id.ToString("00.##")}{(isTest ? "_test" : "")}.txt");
    
}