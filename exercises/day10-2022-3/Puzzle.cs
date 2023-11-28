internal class Puzzle(string rawInput) : AocPuzzle<string[], string>(rawInput)
{
    private IEnumerable<(int Cycle, int Value)> Execute()
    {
        int cycle = 1;
        int curValue = 1;
        yield return (cycle++, curValue);
        foreach (string line in _input)
        {
            switch (line.Split(' '))
            {
                case ["noop"]:
                    yield return (cycle++, curValue);
                    break;
                case ["addx", string value]:
                    yield return (cycle++, curValue);
                    curValue += int.Parse(value);
                    yield return (cycle++, curValue);
                    break;
            }
        }
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override string RunPartOne()
    {
        int sum = 0;
        foreach (var (cycle, value) in Execute())
        {
            if ((cycle - 20) % 40 == 0)
                sum += value * cycle;
        }
        return sum.ToString();
    }

    protected override string RunPartTwo()
    {
        List<char> pixels = [];
        foreach (var (cycle, value) in Execute())
        {
            int x = (cycle - 1) % 40;
            pixels.Add(x >= value - 1 && x <= value + 1 ? '#' : '.');
        }
        var rows = pixels.Take(6 * 40).Chunk(40).Select(row => string.Join("", row));
        return $"\n{string.Join('\n', rows)}";
    }
}
