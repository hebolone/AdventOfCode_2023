namespace Days;

internal class Day01 : Day {

    public override object Basic() => DoWork(_Input);

    public override object Advanced() {
        Dictionary<string, string> digits = new() {
          ["one"] = "o1e",
          ["two"] = "t2o",
          ["three"] = "t3e",
          ["four"] = "f4r",
          ["five"] = "f5e",
          ["six"] = "s6x",
          ["seven"] = "s7n",
          ["eight"] = "e8t",
          ["nine"] = "n9e"
        };
        var converted = new List<string>();
        _Input.ForEach(line => {
            foreach(var digit in digits) {
                if(line.Contains(digit.Key)) {
                    line = line.Replace(digit.Key, digit.Value);
                }
            }
            converted.Add(line);
        });
        
        return DoWork(converted);
    }

    #region Private

    protected static int DoWork(List<string> input) {
        var result = 0;
        input.ForEach(line => {
            var digitLine = string.Empty;
            foreach(char c in line) {
                if(Char.IsDigit(c)) {
                    digitLine += c;
                }
            }
            result += int.Parse($"{digitLine.First()}{digitLine.Last()}");
        });
        return result;
    }

    #endregion

}