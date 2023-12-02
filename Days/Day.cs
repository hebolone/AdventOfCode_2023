namespace Days;

internal abstract class Day {

    public abstract object Basic();

    public abstract object Advanced();

    public bool IsTest { get; set; } = false;

    public void SetInput(List<string> input) => Parse(input);

    protected List<string> _Input = [];

    protected virtual void Parse(List<string> input) { _Input = input; }

}