namespace Builder;

internal class DaysBuilder(string basePath) : IDaysBuilder {

    private readonly string _BasePath = basePath;
    private List<int> _TestDays = new();
    private Dictionary<int, Day> _Days = new();

    #region IDaysBuilder

    public IDaysBuilder AddDay(int id, Day day) {
        if(!_Days.ContainsKey(id)) {
            _Days.Add(id, day);
        } else {
            throw new ArgumentException($"A solver for day '{id}' is already registered.");
        }
        return this;
    }

    public IDaysBuilder Solve(int? id = null, TSolveType solveType = TSolveType.BOTH) {

        var idToSolve = id == null ? _Days.Keys.Max() : (int) id;
        var dayFound = _Days.ContainsKey(idToSolve);

        if(dayFound) {
            //  Read input file
            var solver = _Days[idToSolve];
            var isTest = _TestDays.Contains(idToSolve);
            solver.Input = ReadInput(idToSolve, isTest);

            //  Get solution
            var results = new List<Result>();
            switch(solveType) {
                case TSolveType.BASIC:
                    results.Add(new(idToSolve, solver.Basic(), solveType, isTest));
                    break;
                case TSolveType.ADVANCED:
                    results.Add(new(idToSolve, solver.Advanced(), solveType, isTest));
                    break;
                default:
                    results.Add(new(idToSolve, solver.Basic(), TSolveType.BASIC, isTest));
                    results.Add(new(idToSolve, solver.Advanced(), TSolveType.ADVANCED, isTest));
                    break;
            }

            results.ForEach(r => {
                Console.WriteLine(r.ToString());
            });
        } else {
            var message = $"Day '{idToSolve}' is not present on list.";
            throw new ArgumentException(message);
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

    #region Private

    private List<string> ReadInput(int id, bool isTest) {
    private List<string> ReadInput(int id, bool isTest) {
        //  Check if file exists
        var inputFilePath = GetInputFileName(id, isTest);
        if(!File.Exists(inputFilePath)) {
            throw new FileNotFoundException($"Missing input file: '{inputFilePath}'. Contact Sm3P");
        }

        var retValue = new List<string>();
        var lines = File.ReadLines(inputFilePath);
        
        foreach(var line in lines) {
            retValue.Add(line);
        }

       return retValue;
    }

    private string GetInputFileName(int id, bool isTest) => Path.Combine(_BasePath, $"day_{id:00.##}{(isTest ? "_test" : "")}.txt");
    
    #endregion

}