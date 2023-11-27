internal class Day01 : Day {

    public override object Basic() {
        return 1;
    }

    public override object Advanced() {

        Input.ForEach(l => Console.WriteLine(l));

        return Input.Count;
    }

}