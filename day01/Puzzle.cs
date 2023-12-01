using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

internal class Puzzle(string rawInput) : AocPuzzle<string[], int>(rawInput)
{
    [StringSyntax("regex")]
    private static readonly string _pattern = @"one|two|three|four|five|six|seven|eight|nine|[0-9]";
    private static readonly Regex _reFirst = new(_pattern, RegexOptions.Compiled);
    private static readonly Regex _reLast = new(_pattern, RegexOptions.Compiled | RegexOptions.RightToLeft);
    private static readonly Dictionary<string, int> _valueMap = new()
    {
        ["one"] = 1,
        ["two"] = 2,
        ["three"] = 3,
        ["four"] = 4,
        ["five"] = 5,
        ["six"] = 6,
        ["seven"] = 7,
        ["eight"] = 8,
        ["nine"] = 9
    };

    private static int GetValue(string line)
    {
        var span = line.AsSpan();
        int firstIndex = span.IndexOfAnyInRange('0', '9');
        int lastIndex = span.LastIndexOfAnyInRange('0', '9');
        return (line[firstIndex] - 48) * 10 + line[lastIndex] - 48;
    }

    private static int GetValue2(string line)
    {
        string firstMatch = _reFirst.Match(line).Value;
        if (!_valueMap.TryGetValue(firstMatch, out int firstValue))
            firstValue = firstMatch[0] - 48;

        string lastMatch = _reLast.Match(line).Value;
        if (!_valueMap.TryGetValue(lastMatch, out int lastValue))
            lastValue = lastMatch[0] - 48;

        return firstValue * 10 + lastValue;
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override int RunPartOne() => _input.Select(GetValue).Sum();

    protected override int RunPartTwo() => _input.Select(GetValue2).Sum();
}
