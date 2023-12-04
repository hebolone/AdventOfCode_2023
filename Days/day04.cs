namespace Days;

internal class Day04 : Day {

    private record Card(int Id, int WinningNumbersCount) {
        public int Points => WinningNumbersCount == 0 ? 0 : (int) Math.Pow(2, WinningNumbersCount - 1);
    };
    private readonly List<Card> _Cards = [];
    
    public override object Basic() => _Cards.Sum(c => c.Points);
    
    public override object Advanced() {
        Dictionary<Card, int> dictoCards = [];
        
        _Cards.ForEach(current => {
            //  Add original
            IncrementDictoCards(dictoCards, current);
            //  Add copies
            for(var i = 1; i <= current.WinningNumbersCount; i ++) {
                var next = _Cards.First(c => c.Id == current.Id + i);
                IncrementDictoCards(dictoCards, next, dictoCards[current]);
            }
        });
        
        return dictoCards.Sum(i => i.Value);
    }
    
    #region Protected

    protected override void Parse(List<string> input) {
        Regex regex = new(@"Card\s+(?<card_id>\d+)\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        input.ForEach(line => {
            var parts = line.Split("|");
            var leftPart = parts[0].Split(":");
            var card_id = -1;
            MatchCollection matches = regex.Matches(leftPart[0]);
            foreach (Match match in matches.Cast<Match>()) {
                GroupCollection groups = match.Groups;
                card_id = int.Parse(groups["card_id"].Value);
            }
            List<int> winningNumbers = leftPart[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
            List<int> extractedNumbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i)).ToList();
            var card = new Card(card_id, extractedNumbers.Intersect(winningNumbers).Count());
            _Cards.Add(card);
        });
    }
    
    #endregion
    #region Private
    
    private static void IncrementDictoCards(Dictionary<Card, int> dicto, Card card, int increment = 1) {
        if(dicto.TryGetValue(card, out int value)) {
            dicto[card] = value + increment;
        } else {
            dicto.Add(card, increment);
        }
    }

    #endregion

}