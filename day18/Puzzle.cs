using Coord = (long X, long Y);
using DigStep = (char Direction, int Length, string Color);

internal class Puzzle(string rawInput) : AocPuzzle<DigStep[], long>(rawInput)
{
    private static readonly Dictionary<char, Coord> _deltas = new()
    {
        ['U'] = (0, -1),
        ['R'] = (1, 0),
        ['D'] = (0, 1),
        ['L'] = (-1, 0)
    };

    private static DigStep CreateDigStep(string line)
    {
        var parts = line.Split(' ');
        return (parts[0][0], int.Parse(parts[1]), parts[2][2..^1]);
    }

    private static DigStep SwapDigStep(DigStep step)
    {
        char direction = step.Color[^1] switch
        {
            '0' => 'R',
            '1' => 'D',
            '2' => 'L',
            '3' => 'U',
            _ => throw new System.Diagnostics.UnreachableException()
        };
        return new(direction, Convert.ToInt32(step.Color[..^1], 16), step.Color);
    }

    private static long GetVolume(DigStep[] steps)
    {
        Coord pos = (0, 0);
        List<Coord> vertices = [];
        foreach (var step in steps)
        {
            vertices.Add(pos);
            var (deltaX, deltaY) = _deltas[step.Direction];
            pos.X += deltaX * step.Length;
            pos.Y += deltaY * step.Length;
        }

        long volume = 0;
        for (int i = 1; i < vertices.Count; ++i)
            volume += vertices[i - 1].X * vertices[i].Y - vertices[i].X * vertices[i - 1].Y;

        return Math.Abs(volume / 2) + steps.Sum(step => step.Direction is 'L' or 'U' ? step.Length : 0) + 1;
    }


    protected override DigStep[] ParseInput(string rawInput) => rawInput.Split('\n').Select(CreateDigStep).ToArray();

    protected override long RunPartOne() => GetVolume(_input);

    protected override long RunPartTwo() => GetVolume(_input.Select(SwapDigStep).ToArray());
}
