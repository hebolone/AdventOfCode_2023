namespace Days;

internal abstract class Day {

    public abstract object Basic();

    public abstract object Advanced();

    public List<string> Input { get; set; } = [];

    public bool IsTest { get; set; } = false;

}