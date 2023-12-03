using System.Text.RegularExpressions;

internal class Puzzle(string rawInput) : AocPuzzle<EngineSchematic, int>(rawInput)
{
    private static readonly Regex _re = new(@"(\d+|[^.])", RegexOptions.Compiled);

    protected override EngineSchematic ParseInput(string rawInput)
    {
        List<SchematicObject<int>> numbers = [];
        List<SchematicObject<char>> symbols = [];
        var lines = rawInput.Split('\n');
        for (int y = 0; y < lines.Length; ++y)
        {
            var matches = _re.Matches(lines[y]);
            foreach (Match match in matches)
            {
                Coord pos = (match.Index, y);
                if (int.TryParse(match.Value, out int number))
                    numbers.Add(new SchematicObject<int>(pos, number, match.Length));
                else
                    symbols.Add(new SchematicObject<char>(pos, match.Value[0], 1));
            }
        }

        return new(numbers.ToArray(), symbols.ToArray());
    }

    protected override int RunPartOne()
        => _input.Numbers
            .Where(num => _input.Symbols.Any(sym => num.IsAdjacentTo(sym)))
            .Sum(num => num.Value);

    protected override int RunPartTwo()
    {
        int result = 0;
        foreach (var gear in _input.Symbols.Where(sym => sym.Value == '*'))
        {
            var adjacentParts = _input.Numbers.Where(num => num.IsAdjacentTo(gear)).ToArray();
            if (adjacentParts.Length == 2)
                result += adjacentParts[0].Value * adjacentParts[1].Value;
        }
        return result;
    }
}
