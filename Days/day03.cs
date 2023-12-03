namespace Days;

internal class Day03 : Day {

    private record Number(Coordinate StartPosition) {
        public int Chars = 0;
        public int TotalValue = 0;
        public bool IsDomain(int indexStart, int indexToCheck) {
            List<int> listIndexesOfDomain = [];
            for(int i = 0; i < Chars; i ++) {
                listIndexesOfDomain.Add(indexStart + i);
            }
            return listIndexesOfDomain.Contains(indexToCheck);
        }
    }
    private record Gear(Number Number1, Number Number2);
    private Board<char> _Board;
    private List<Number> _Numbers = [];

    public override object Basic() => _Numbers.Where(n => CheckIfValidNumber(n)).Sum(n => n.TotalValue);
    
    public override object Advanced() {
        int retValue = 0;
        var gears = FindGears();
        gears.ForEach(g => {
            retValue += g.Number1.TotalValue * g.Number2.TotalValue;
        });
        return retValue;
    }

    #region Protected

    protected override void Parse(List<string> input) {
        //  Get size of this board
        _Board = new(input.First().Length, input.Count());
        var counter = 0;

        input.ForEach(line => {
            Number? currentNumber = null;
            foreach(char c in line) {
                _Board.SetValue(counter, c);
                if(char.IsDigit(c)) {
                    //  Check if I'm inside a number
                    if(currentNumber == null) {
                        currentNumber = new(_Board.ConvertIndexToCoordinates(counter)) { Chars = 1};
                    } else {
                        currentNumber.Chars ++;
                    }
                    //  If this is the last char in this line, then I have to close current number
                    if((counter + 1) % _Board.X == 0) {
                        CloseNumber(currentNumber);
                        _Numbers.Add(currentNumber);
                    }
                } else {
                    if(currentNumber != null) {
                        CloseNumber(currentNumber);
                        _Numbers.Add(currentNumber);
                    }
                    currentNumber = null;
                }
                counter ++;
            }
        });

        //Console.WriteLine(_Board.PrintBoard());
    }

    #endregion

    #region Private

    private bool CheckIfValidNumber(Number number) {
        var currentCoordinate = number.StartPosition;
        _Board.GetSurrounding(currentCoordinate);

        var isValid = false;
        var surroundingCells = GetSurroundingOfWholeNumber(number);
        surroundingCells.ForEach(c => {
            var cellValue = _Board[c];
            if(cellValue != '.' && !char.IsDigit(cellValue)) {
                isValid = true;
            }
        });
        
        return isValid;
    }

    private List<Coordinate> GetSurroundingOfWholeNumber(Number number) {
        List<Coordinate> retValue = [];
        var startPosition = _Board.ConvertCoordinatesToIndex(number.StartPosition);
        var i = 0;
        while(i < number.Chars) {
            var surroundingCells = _Board.GetSurrounding(startPosition + i);
            surroundingCells.ForEach(c => {
                if(!retValue.Contains(c)) {
                    retValue.Add(c);
                }
            });
            i ++;
        }
        return retValue;
    }

    private void CloseNumber(Number number) {
        //  Calculate value of this number
        var stringNumber = string.Empty;
        var startPosition = _Board.ConvertCoordinatesToIndex(number.StartPosition);
        var i = 0;
        while(i < number.Chars) {
            stringNumber += _Board[startPosition + i];
            i ++;
        }
        number.TotalValue = int.Parse(stringNumber);
    }

    private List<Gear> FindGears() {
        List<Gear> retValue = [];
        var myEnum = _Board.GetEnumerator();
        var index = 0;
        while(myEnum.MoveNext()) {
            if(myEnum.Current == '*') {
                //  Gear found. Is it valid?
                var gear = CheckGear(index);
                if(gear != null) {
                    retValue.Add(gear);
                }
            }
            index++;
        }
        return retValue;
    }

    private Gear? CheckGear(int index) {
        Gear? retValue = null;
        var surroundingCells = _Board.GetSurrounding(index);
        //  Check if any surrounding cell is part of a number
        List<Number> surroundingNumbers = [];
        surroundingCells.ForEach(cell => {
            _Numbers.ForEach(n => {
                if(n.IsDomain(_Board.ConvertCoordinatesToIndex(n.StartPosition), _Board.ConvertCoordinatesToIndex(cell))) {
                    if(!surroundingNumbers.Contains(n)) {
                        surroundingNumbers.Add(n);
                    }
                }
            });
        });
        if(surroundingNumbers.Count == 2) {
            retValue = new(surroundingNumbers[0], surroundingNumbers[1]);
        }
        return retValue;
    }

    #endregion

}