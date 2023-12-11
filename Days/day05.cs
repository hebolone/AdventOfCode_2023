using System.ComponentModel;

namespace Days;

internal class Day05 : Day {

    private record Converter(long Destination, long Source, long Range) {
        public (long From, long To) ConvertedRange => (Source, Source + Range - 1);
        public long Delta => Destination - Source;
        public override string ToString() => $"{Destination} {Source} {Range} ({ConvertedRange.From}-{ConvertedRange.To},{Delta})";
    }
    private record SeedRange(long From, long Length);
    private class DictoLevels : Dictionary<int, List<Converter>> {
        public int CurrentLevel { get; set; } = 0;
        public List<Converter>? CurrentConverters => CurrentLevel > 0 && CurrentLevel <= this.Count() ? this[CurrentLevel] : null;
        public int? MoveNext() => CurrentLevel < this.Count ? ++CurrentLevel : null;
        public void ResetLevels() => CurrentLevel = 0;
        public void AddConverter(int level, Converter converter) {
            if(this.ContainsKey(level)) {
                this[level].Add(converter);
            } else {
                this.Add(level, [ converter ]);
            }
        }
    }
    private readonly DictoLevels _DictoLevels = [];
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
        var minimum = 0L;

        _DictoLevels.ResetLevels();
        Tree<Converter> tree = new();
        //tree.AddNode(new Converter(79, 79, 92));
        //GetConvertersTree(tree, 79, 92);

        //IEnumerable<Converter> destinationConverters;
        var from = 79L;
        var to = 92L;
        IEnumerable<Converter>? converters;
        Console.WriteLine($"Starting -> from = {from}, to = {to}");
        while(_DictoLevels.MoveNext() != null) {
            converters = _DictoLevels.CurrentConverters;
            if(converters == null) {
                throw new Exception($"No converters found at level {_DictoLevels.CurrentLevel}");
            } else {
                converters = SelectConverterInRange(converters!, from, to);

                //tree.AddNode()

                if(converters.Count() > 1) {
                    foreach(var conv in converters) {
                        Console.WriteLine($" -> {conv}");
                        //  Let's split
                        if(conv.ConvertedRange.From <= from && conv.ConvertedRange.To >= from) {
                            Console.WriteLine($" --> {from}, {conv.ConvertedRange.To} ({conv.Delta})");
                        } else if(conv.ConvertedRange.From <= to && conv.ConvertedRange.To >= to) {
                            Console.WriteLine($" --> {conv.ConvertedRange.From}, {to} ({conv.Delta})");
                            
                        }
                        //var pFrom = Convert(from, converters.ToList());
                    }
                    Console.WriteLine($"");
                }

                from = Convert(from, converters.ToList());
                to = Convert(to, converters.ToList());
                Console.WriteLine($"Level {_DictoLevels.CurrentLevel} -> Converters found: {converters?.Count() ?? 0}, from = {from}, to = {to}");
                //foreach(var conv in converters) {
                //}

            }
            //var acceptableConverters = SelectConverterInRange(converters, Convert(from, ))
        }

        Console.WriteLine($"Final result: {from}");

        // var selectedLevel = _DictoLevels.Count;
        // var currentConverter = SelectLowestConverter(_TransformationLevel[_DictoLevels[selectedLevel]]);

        // Console.WriteLine($"Lowest converter: {currentConverter}");

        // while(selectedLevel > 1) {
        //     selectedLevel --;
        //     var availableConverters = _TransformationLevel[_DictoLevels[selectedLevel]];
        //     //  Choose converter which gives me correct range
        //     var from = currentConverter.ConvertedRange.From;
        //     var to = currentConverter.ConvertedRange.To;
        //     currentConverter = SelectConverterInRange(availableConverters, currentConverter);
        //     Console.WriteLine($"[{selectedLevel}] -> {currentConverter}");
        // }

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
        var currentLevel = -1;
        input.ForEach(line => {
            if(line.Length > 0) {
                var headerLine = Char.IsAsciiLetter(line[0]);
                if(headerLine) {
                    if(currentLevel == -1) {
                        //  Seeds section
                        var seeds = line.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        _Seeds = seeds.Select(s => long.Parse(s)).ToList();
                    }
                    currentLevel ++;
                } else {
                    var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var converter = new Converter(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
                    _DictoLevels.AddConverter(currentLevel, converter);
                }
            }
        });
    }
    
    #endregion

    #region Private
    
    // private void AddToTransformationLevel(string level, Converter converter) {
    //     if(_TransformationLevel.TryGetValue(level, out var Value)) {
    //         Value.Add(converter);
    //     } else {
    //         _TransformationLevel.Add(level, [ converter ]);
    //     }
    // }

    private long Convert(long input, List<Converter> converters) {
        var retValue = input;
        converters.ForEach(c => {
            if(input >= c.Source && input < c.Source + c.Range) {
                retValue = c.Destination + input - c.Source;
            }
            // if(input >= c.ConvertedRange.From && input < c.ConvertedRange.To) {
            //     retValue = input + c.Delta;
            // }
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
            var converters = _DictoLevels.CurrentConverters;
            currentSeed = Convert(currentSeed, converters!);
        }
        _DictoLevels.ResetLevels();
        return currentSeed;
    }

    // private Tree<Converter> GetConvertersTree(Tree<Converter> tree, long from, long to) {

    //     IEnumerable<Converter> destinationConverters;
    //     while(_DictoLevels.MoveNext() != null) {

    //         var converters = _DictoLevels.Current;
    //         destinationConverters = SelectConverterInRange(converters, currentConverter);
    //     }

    //     return retValue;
    // }
    
    // private Converter SelectLowestConverter(List<Converter> converters) => converters.OrderBy(c => c.Destination).First();

    private IEnumerable<Converter> SelectConverterInRange(IEnumerable<Converter> converters, long from, long to) =>
        //  Select a converter which gives me a result between from and to
        //  This is an intersection
        converters.Where(
            c => {
                var x1 = c.ConvertedRange.From;
                var x2 = c.ConvertedRange.To;
                var y1 = from;
                var y2 = to;
                // var y1 = cd.Source;
                // var y2 = cd.Source + cd.Range - 1;
                return 
                    (x1 >= y1 && x1 <= y2) ||
                    (x2 >= y1 && x2 <= y2) ||
                    (y1 >= x1 && y1 <= x2) ||
                    (y2 >= x1 && y2 <= x2);
            }
        );
    
    #endregion

}