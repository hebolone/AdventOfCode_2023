using System.Collections;

internal record Coordinate(int X, int Y);

internal class Board<T> : IEnumerable<T> {

    private readonly int _X;
    private readonly int _Y;
    private readonly List<T> _Cells = [];
    private readonly Func<T, string> _PrintFunc = (i) => i?.ToString() ?? ".";

    public Board(int x, int y, Func<T>? initializer = null) {
        _X = x;
        _Y = y;
        for(int i = 0; i < (_X * _Y); i ++) {
            T? t = (initializer == null ? default : initializer()) ?? throw new ArgumentNullException($"Argument of type {typeof(T)} must have an explicit initializer.");
            _Cells.Add(t);
        } 
    }

    #region Properties

    public int Size => _X * _Y;
    public int X => _X;
    public int Y => _Y;
    public T this[int i] => _Cells[i];
    public T this[int xFrom, int yFrom] => this[xFrom + yFrom * _X];
    public T this[Coordinate coordinate] => this[ConvertCoordinatesToIndex(coordinate)];
    public void SetValue(int xFrom, int yFrom, T t) => SetValue(xFrom + yFrom * _X, t);
    public void SetValue(int index, T t) => _Cells[index] = t;
    public void SetValue(Coordinate coordinate, T t) => SetValue(coordinate.X, coordinate.Y, t);
    public List<Coordinate> GetSurrounding(int index) {
        var actual = ConvertIndexToCoordinates(index);
        List<Coordinate> retValue = [];
        List<Coordinate> surroundingCells = [
            new Coordinate(actual.X - 1, actual.Y - 1),
            new Coordinate(actual.X, actual.Y - 1),
            new Coordinate(actual.X + 1, actual.Y - 1),
            new Coordinate(actual.X - 1, actual.Y),
            new Coordinate(actual.X + 1, actual.Y),
            new Coordinate(actual.X - 1, actual.Y + 1),
            new Coordinate(actual.X, actual.Y + 1),
            new Coordinate(actual.X + 1, actual.Y + 1)
        ];
        surroundingCells.ForEach(c => {
            if(!IsOutside(c)) {
                retValue.Add(c);
            }
        });
        return retValue;
    }

    public List<Coordinate> GetSurrounding(Coordinate coordinate) => GetSurrounding(ConvertCoordinatesToIndex(coordinate));

    #endregion

    #region Public

    public string PrintBoard(Func<T, string>? printFunc = null) {
        var sb = new StringBuilder();
        for(int y = 0; y < _Y; y ++) {
            for(int x = 0; x < _X; x ++) {
                var printer = printFunc ?? _PrintFunc;
                sb.Append(printer(this[x, y]));
            }
            sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    public Coordinate ConvertIndexToCoordinates(int index) {
        var y_derived = index / _Y;
        var x_derived = index - (_Y * y_derived);
        return new(x_derived, y_derived); 
    }

    public int ConvertCoordinatesToIndex(Coordinate coordinate) => coordinate.X + coordinate.Y * _X;


    #endregion

    #region Private

    private bool IsOutside(Coordinate coordinate) => coordinate.X < 0 || coordinate.Y < 0 || coordinate.X > _X - 1 || coordinate.Y > _Y - 1;

    #endregion

    #region IEnumerable

    public IEnumerator<T> GetEnumerator() => _Cells.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _Cells.GetEnumerator();

    #endregion

}