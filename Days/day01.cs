using System.Collections.Frozen;
using System.Globalization;

namespace Days;

internal class Day01 : Day {

    public override object Basic() => Parse(Input);

    public override object Advanced() {
        var digits = new List<string>() {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        var converted = new List<string>();
        Input.ForEach(line => {
            var firstOccurrence = line.Length;
            var indexes = new Dictionary<int, int>();
            foreach(var digit in digits) {
                if(line.Contains(digit)) {
                    indexes.Add(line.IndexOf(digit), digits.IndexOf(digit) + 1);
                }
            }
            
            foreach(var d in indexes.OrderBy(v => v.Key).Select(i => i.Value)) {
                line = line.Replace(digits[d - 1], d.ToString());
            }
            converted.Add(line);
        });
        
        return Parse(converted);
    }

    #region Private

    private static int Parse(List<string> input) {
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