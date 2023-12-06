using System.Text.RegularExpressions;
using Race = (long Time, long Distance);

internal class Puzzle(string rawInput) : AocPuzzle<Race[], long>(rawInput)
{
    private static readonly Regex _re = new(@"\s+", RegexOptions.Compiled);

    private static long GetWinCount(Race race)
    {
        long low = 1;
        while ((race.Time - low) * low <= race.Distance)
            ++low;

        long high = race.Time - 1;
        while ((race.Time - high) * high <= race.Distance)
            --high;

        return high - low + 1;
    }


    protected override Race[] ParseInput(string rawInput)
    {
        var lines = rawInput.Split('\n');
        var times = _re.Split(lines[0].Split(':')[1].Trim()).Select(long.Parse);
        var distances = _re.Split(lines[1].Split(':')[1].Trim()).Select(long.Parse);
        return times.Zip(distances).ToArray();
    }

    protected override long RunPartOne() => _input.Select(GetWinCount).Aggregate((prev, cur) => prev * cur);

    protected override long RunPartTwo()
    {
        long time = long.Parse(string.Join("", _input.Select(race => race.Time)));
        long distance = long.Parse(string.Join("", _input.Select(race => race.Distance)));
        return GetWinCount((time, distance));
    }
}
