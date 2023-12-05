using System.ComponentModel;

namespace Days;

internal class Day05 : Day {

    private record Converter(int Destination, int Source, int Range);
    private readonly Dictionary<string, List<Converter>> _TransformationLevel = [];
    private class DictoLevels : Dictionary<int, string> {
        public int CurrentLevel { get; set; } = 0;
        public int MoveNext() => ++CurrentLevel;
    }
    private readonly DictoLevels _DictoLevels = new() {
        [0] = "seeds:", 
        [1] = "seed-to-soil map:",
        [2] = "soil-to-fertilizer map:", 
        [3] = "fertilizer-to-water map:", 
        [4] = "water-to-light map:", 
        [5] = "light-to-temperature map:", 
        [6] = "temperature-to-humidity map:",
        [7] = "humidity-to-location map:"
    };

    public override object Basic() {
        //  First level is "seeds"

        //  Proceed to next level

        return -1;
    }
    
    public override object Advanced() => -1;
    
    #region Protected

    protected override void Parse(List<string> input) {
        List<string> acceptedHeaders = [.. _DictoLevels.Values];

        var currentLevel = string.Empty;
        input.ForEach(line => {
            if(line.Length > 0) {
                //  Check if this the header or data section
                var isDataSection = Char.IsDigit(line[0]);
                if(isDataSection) {
                    var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var converter = new Converter(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    AddToTransformationLevel(currentLevel, converter);
                } else {
                    if(acceptedHeaders.Contains(line)) {
                        currentLevel = line;
                    }
                }
            }
        });
    }
    
    #endregion

    #region Private
    
    private void AddToTransformationLevel(string level, Converter converter) {
        if(_TransformationLevel.TryGetValue(level, out var Value)) {
            Value.Add(converter);
        } else {
            _TransformationLevel.Add(level, [ converter ]);
        }
    }

    #endregion

}