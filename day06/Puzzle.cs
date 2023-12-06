using System.Text.RegularExpressions;
using Race = (long Time, long Distance);

internal class Puzzle(string rawInput) : AocPuzzle<Race[], int>(rawInput)
{
    private static readonly Regex _re = new(@"\s+", RegexOptions.Compiled);

    private static int GetWinCount(Race race)
    {
        int count = 0;
        for (int time = 1; time < race.Time; ++time)
        {
            if ((race.Time - time) * time > race.Distance)
                ++count;
        }
        return count;
    }


    protected override Race[] ParseInput(string rawInput)
    {
        var lines = rawInput.Split('\n');
        var times = _re.Split(lines[0].Split(':')[1].Trim()).Select(long.Parse);
        var distances = _re.Split(lines[1].Split(':')[1].Trim()).Select(long.Parse);
        return times.Zip(distances).ToArray();
    }

    protected override int RunPartOne() => _input.Select(GetWinCount).Aggregate((prev, cur) => prev * cur);

    protected override int RunPartTwo()
    {
        long time = long.Parse(string.Join("", _input.Select(race => race.Time)));
        long distance = long.Parse(string.Join("", _input.Select(race => race.Distance)));
        return GetWinCount((time, distance));
    }
}
