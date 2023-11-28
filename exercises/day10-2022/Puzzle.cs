internal class Puzzle(string rawInput) : AocPuzzle<string[], string>(rawInput)
{
    private static List<int> ExecuteLine(List<int> values, string line) => line.Split(' ') switch
    {
        ["noop"] => [..values, values[^1]],
        ["addx", string value] => [..values, values[^1], values[^1] + int.Parse(value)],
        _ => values
    };

    private static int GetSignalStrength(List<int> values, int cycle) => cycle * values[cycle - 1];

    private static char GetPixelValue(int value, int cycle) => (cycle % 40) switch
    {
        int pos when pos >= value - 1 && pos <= value + 1 => '#',
        _ => '.'
    };


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override string RunPartOne()
    {
        var values = _input.Aggregate((List<int>)[1], ExecuteLine);

        return ((int[])[20, 60, 100, 140, 180, 220])
            .Select(cycle => GetSignalStrength(values, cycle))
            .Sum()
            .ToString();
    }

    protected override string RunPartTwo()
    {
        var rows = _input.Aggregate((List<int>)[1], ExecuteLine)
            .Take(6 * 40)
            .Select(GetPixelValue)
            .Chunk(40)
            .Select(row => string.Join("", row));

        return $"\n{string.Join('\n', rows)}";
    }
}
