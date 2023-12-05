using System.ComponentModel;

namespace Days;

internal class Day05 : Day {

    private record Converter(long Destination, long Source, long Range) {
        public (long From, long To) ConvertedRange => (Destination, Destination + Range - 1);
        public override string ToString() => $"{Destination} {Source} {Range}";
    }
    private record SeedRange(long From, long Length);
    private readonly Dictionary<string, List<Converter>> _TransformationLevel = [];
    private class DictoLevels : Dictionary<int, string> {
        public int CurrentLevel { get; set; } = 0;
        public int? MoveNext() => CurrentLevel < this.Count ? ++CurrentLevel : null;
        public string CurrentValue => this[CurrentLevel];
        public void ResetLevels() { CurrentLevel = 0; }
    }
    private readonly DictoLevels _DictoLevels = new() {
        [1] = "seed-to-soil map:",
        [2] = "soil-to-fertilizer map:", 
        [3] = "fertilizer-to-water map:", 
        [4] = "water-to-light map:", 
        [5] = "light-to-temperature map:", 
        [6] = "temperature-to-humidity map:",
        [7] = "humidity-to-location map:"
    };
    private List<long> _Seeds = [];
    private List<SeedRange> _SeedRanges = [];

    public override object Basic() {
        List<long> results = [];
        _Seeds.ForEach(s => {
            results.Add(ConvertAll(s));
        });
        
        return results.Min();
    }
    
    public override object Advanced() {
        var minimum = 0L; //long.MaxValue;
        
        var selectedLevel = _DictoLevels.Count;
        var currentConverter = SelectLowestConverter(_TransformationLevel[_DictoLevels[selectedLevel]]);

        Console.WriteLine($"Lowest converter: {currentConverter}");

        while(selectedLevel > 1) {
            selectedLevel --;
            var availableConverters = _TransformationLevel[_DictoLevels[selectedLevel]];
            //  Choose converter which gives me correct range
            var from = currentConverter.ConvertedRange.From;
            var to = currentConverter.ConvertedRange.To;
            currentConverter = SelectConverterInRange(availableConverters, currentConverter);
            Console.WriteLine($"[{selectedLevel}] -> {currentConverter}");
        }

        //  Choose properly seed
        // _Seeds.First(s => s.From)

        // for(long i = currentConverter.Source; i < s.From + s.Length - 1; i ++) {
        //     var partial = ConvertAll(i);
        //     if(partial < minimum) {
        //         minimum = partial;
        //     }
        // }

        return minimum;
    }
    
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
                    var converter = new Converter(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
                    AddToTransformationLevel(currentLevel, converter);
                } else {
                    if(acceptedHeaders.Contains(line)) {
                        currentLevel = line;
                    } else {
                        //  Seeds section
                        // var seeds = line.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        // _Seeds = seeds.Select(s => long.Parse(s)).ToList();
                        var seeds = line.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        ParseSeedsBasic(seeds);
                        ParseSeedsAdvanced(seeds);
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

    private long Convert(long input, List<Converter> converters) {
        var retValue = input;
        converters.ForEach(c => {
            if(input >= c.Source && input < c.Source + c.Range) {
                retValue = c.Destination + input - c.Source;
            }
        });	
        return retValue;
    }

    private void ParseSeedsBasic(string[]? seedsDatas) {
        if(seedsDatas != null) {
            _Seeds = seedsDatas.Select(s => long.Parse(s)).ToList();
        } else {
            throw new ArgumentNullException("Invalid seeds datas.");
        }
    }

    private void ParseSeedsAdvanced(string[]? seedsDatas) {
        if(seedsDatas != null) {
            for(int i = 0; i < seedsDatas.Length; i += 2) {
                _SeedRanges.Add(new(long.Parse(seedsDatas[i]), long.Parse(seedsDatas[i + 1])));
            }
        } else {
            throw new ArgumentNullException("Invalid seeds datas.");
        }
    }

    private long ConvertAll(long input) {
        var currentSeed = input;
        while(_DictoLevels.MoveNext() != null) {
            var converters = _TransformationLevel[_DictoLevels.CurrentValue];
            currentSeed = Convert(currentSeed, converters);
        }
        _DictoLevels.ResetLevels();
        return currentSeed;
    }
    
    private Converter SelectLowestConverter(List<Converter> converters) => converters.OrderBy(c => c.Destination).First();

    private Converter SelectConverterInRange(List<Converter> converters, Converter cd) {
        //  Select a converter which gives me a result between from and to
        //  This is an intersection
        var converterFound = converters.FirstOrDefault(
            c => {
                var x1 = c.ConvertedRange.From;
                var x2 = c.ConvertedRange.To;
                var y1 = cd.Source;
                var y2 = cd.Source + cd.Range - 1;
                return 
                    (x1 >= y1 && x1 <= y2) ||
                    (x2 >= y1 && x2 <= y2) ||
                    (y1 >= x1 && y1 <= x2) ||
                    (y2 >= x1 && y2 <= x2);
            }
        );
        return converterFound ?? new (cd.Source, cd.Source, cd.Range);
    }
    
    #endregion

}