using SupportIndex = System.Collections.Generic.Dictionary<Brick, System.Collections.Generic.HashSet<Brick>>;

internal class Puzzle(string rawInput) : AocPuzzle<Brick[], int>(rawInput)
{
    private static Brick CreateBrick(string definition)
    {
        var parts = definition.Split('~');
        var start = parts[0].Split(',').Select(int.Parse).ToArray();
        var end = parts[1].Split(',').Select(int.Parse).ToArray();
        return new((start[0], start[1], start[2]), (end[0], end[1], end[2]));
    }

    private void Fall()
    {
        Brick[] fallingBricks = _input;
        List<Brick> stoppedBricks = [];
        do
        {
            List<Brick> nextFallingBricks = [];
            foreach (var brick in fallingBricks)
            {
                if (brick.MinZ == 1)
                {
                    stoppedBricks.Add(brick);
                    continue;
                }

                bool isFalling = true;
                foreach (var stoppedBrick in stoppedBricks)
                {
                    if (brick.RestsOn(stoppedBrick))
                    {
                        isFalling = false;
                        stoppedBricks.Add(brick);
                        break;
                    }
                }

                if (isFalling)
                {
                    brick.Fall();
                    nextFallingBricks.Add(brick);
                }
            }
            fallingBricks = nextFallingBricks.ToArray();
        } while (stoppedBricks.Count < _input.Length);
    }

    private (SupportIndex Supports, SupportIndex SupportedBy) GetSupportIndices()
    {
        SupportIndex supports = [];
        SupportIndex supportedBy = [];
        foreach (var brick in _input)
        {
            supports[brick] = [];
            supportedBy[brick] = [];
            foreach (var otherBrick in _input)
            {
                if (brick.RestsOn(otherBrick))
                {
                    supports[otherBrick].Add(brick);
                    supportedBy[brick].Add(otherBrick);
                }
            }
        }

        return (supports, supportedBy);
    }


    protected override Brick[] ParseInput(string rawInput) => rawInput.Split('\n').Select(CreateBrick).OrderBy(brick => brick.MinZ).ToArray();

    protected override int RunPartOne()
    {
        Fall();
        var (supports, supportedBy) = GetSupportIndices();
        return _input.Count(brick => supports[brick].Count == 0 || supports[brick].All(supportedBrick => supportedBy[supportedBrick].Count > 1));
    }

    protected override int RunPartTwo()
    {
        Fall();
        var (supports, supportedBy) = GetSupportIndices();
        var startBricks = _input.Where(brick => supports[brick].Any(supportedBrick => supportedBy[supportedBrick].Count == 1));

        int count = 0;
        foreach (var startBrick in startBricks)
        {
            var curSupportedBy = supportedBy.ToDictionary(item => item.Key, item => new HashSet<Brick>(item.Value));
            Brick[] fallingBricks = [startBrick];
            do
            {
                List<Brick> nextFallingBricks = [];
                foreach (var fallingBrick in fallingBricks)
                {
                    foreach (var supportedBrick in supports[fallingBrick])
                    {
                        var supportingBricks = curSupportedBy[supportedBrick];
                        supportingBricks.Remove(fallingBrick);
                        if (supportingBricks.Count == 0)
                            nextFallingBricks.Add(supportedBrick);
                    }
                }

                count += nextFallingBricks.Count;
                fallingBricks = nextFallingBricks.ToArray();
            } while (fallingBricks.Length > 0);
        }

        return count;
    }
}
