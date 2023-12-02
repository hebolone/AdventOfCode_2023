namespace Days;

internal class Day02 : Day {

    private record Game(int Id, List<Set> Sets) {

        public int Power() => GetMaximumPerColor(TColor.RED) * GetMaximumPerColor(TColor.GREEN) * GetMaximumPerColor(TColor.BLUE);
        
        private int GetMaximumPerColor(TColor color) {
            var listOfColorsLaunches = new List<int>();
            foreach(var set in Sets) {
                listOfColorsLaunches.Add(set.GetColor(color));
            }
            return listOfColorsLaunches.Max();
        }
    
    };
    private record Set() {
        public List<Launch> Launches { get; init; } = [];
        public int GetColor(TColor color) => Launches.FirstOrDefault(l => l.Color == color)?.Number ?? 0;
    };
    private record Launch(TColor Color, int Number);
    private enum TColor { RED, GREEN, BLUE }
    private readonly List<Game> _Games = [];

    public override object Basic() {
        Set question = new() {
            Launches = [
                new(TColor.RED, 12),
                new(TColor.GREEN, 13),
                new(TColor.BLUE, 14)
            ]
        };

        var validGames = GetValidGames(question);

        return validGames.Sum();
    }

    public override object Advanced() {
        var powers = new List<int>();
        _Games.ForEach(g => powers.Add(g.Power()));

        return powers.Sum();
    }

    #region Protected

    protected override void Parse(List<string> input) {
        Regex regex = new(@"Game (?<game_id>\d+):(?<sets>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        input.ForEach(l => {
            MatchCollection matches = regex.Matches(l);
            foreach (Match match in matches.Cast<Match>()) {
                GroupCollection groups = match.Groups;
                var game_id = int.Parse(groups["game_id"].Value);
                var sets = ParseSets(groups["sets"].Value);
                _Games.Add(new(game_id, sets));
            }
        });
    }

    #endregion

    #region Private

    private static List<Set> ParseSets(string input) {
        var retValue = new List<Set>();
        var launches = input.Split(";");
        Regex regex = new(@"\s*(?<dice>\d+)\s*(?<color>\w+)\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        foreach(var launch in launches) {
            MatchCollection matches = regex.Matches(launch);
            var launchesList = new List<Launch>();
            foreach (Match match in matches.Cast<Match>()) {
                GroupCollection groups = match.Groups;
                var color = (TColor) Enum.Parse(typeof(TColor), groups["color"].Value, true);
                var dice = int.Parse(groups["dice"].Value);
                launchesList.Add(new Launch(color, dice));
            }
            retValue.Add(new() { Launches =launchesList });
        }

        return retValue;
    }

    private List<int> GetValidGames(Set question) {
        var retValue = new List<int>();
        _Games.ForEach(g => {
            //  Sum of each color
            var valid = true;
            g.Sets.ForEach(s => {
                if( 
                    question.GetColor(TColor.RED) < s.GetColor(TColor.RED) 
                    ||
                    question.GetColor(TColor.GREEN) < s.GetColor(TColor.GREEN) 
                    ||
                    question.GetColor(TColor.BLUE) < s.GetColor(TColor.BLUE) 
                ) {
                    valid = false;
                } 
            });
            
            if(valid) {
                retValue.Add(g.Id);
            }
        });
        return retValue;
    }

    #endregion

}