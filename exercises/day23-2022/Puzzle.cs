using System.Collections.Frozen;
using System.Runtime.InteropServices;
using Coord = (int X, int Y);

internal class Puzzle(string rawInput) : AocPuzzle<HashSet<Coord>, int>(rawInput)
{
    enum Direction { North, South, West, East, NorthEast, NorthWest, SouthEast, SouthWest }

    private static readonly FrozenDictionary<Direction, Coord> _deltas = new Dictionary<Direction, Coord>
    {
        [Direction.North] = (0, -1),
        [Direction.South] = (0, 1),
        [Direction.West] = (-1, 0),
        [Direction.East] = (1, 0),
        [Direction.NorthEast] = (1, -1),
        [Direction.NorthWest] = (-1, -1),
        [Direction.SouthEast] = (1, 1),
        [Direction.SouthWest] = (-1, 1)
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Direction, Direction[]> _lookDirections = new Dictionary<Direction, Direction[]>
    {
        [Direction.North] = [Direction.North, Direction.NorthEast, Direction.NorthWest],
        [Direction.South] = [Direction.South, Direction.SouthEast, Direction.SouthWest],
        [Direction.West] = [Direction.West, Direction.NorthWest, Direction.SouthWest],
        [Direction.East] = [Direction.East, Direction.NorthEast, Direction.SouthEast]
    }.ToFrozenDictionary();

    private static readonly Direction[] _directions = Enum.GetValues<Direction>();
    private static readonly Direction[] _moveDirections = [Direction.North, Direction.South, Direction.West, Direction.East];

    private int PerformRound(int dirOffset)
    {
        var proposals = new Dictionary<Coord, Coord>();
        var destinations = new Dictionary<Coord, int>();
        var otherElves = new HashSet<Coord>();

        foreach (var elf in _input)
        {
            otherElves.Clear();
            foreach (var dir in _directions)
            {
                var pos = (elf.X + _deltas[dir].X, elf.Y + _deltas[dir].Y);
                if (_input.Contains(pos))
                    otherElves.Add(pos);
            }

            if (otherElves.Count == 0)
                continue;

            int dirCounter = dirOffset;
            foreach (var _ in _moveDirections)
            {
                bool canMove = true;
                var moveDirection = _moveDirections[dirCounter % _moveDirections.Length];
                foreach (var lookDirection in _lookDirections[moveDirection])
                {
                    if (otherElves.Contains((elf.X + _deltas[lookDirection].X, elf.Y + _deltas[lookDirection].Y)))
                    {
                        canMove = false;
                        break;
                    }
                }

                if (canMove)
                {
                    var pos = (elf.X + _deltas[moveDirection].X, elf.Y + _deltas[moveDirection].Y);
                    proposals[elf] = pos;
                    ++CollectionsMarshal.GetValueRefOrAddDefault(destinations, pos, out var _);
                    break;
                }

                ++dirCounter;
            }
        }

        int moveCount = 0;
        foreach (var (oldPos, newPos) in proposals)
        {
            if (destinations[newPos] == 1) {
                _input.Remove(oldPos);
                _input.Add(newPos);
                ++moveCount;
            }
        }

        return moveCount;
    }


    protected override HashSet<Coord> ParseInput(string rawInput)
    {
        var elves = new HashSet<Coord>();
        var lines = rawInput.Split('\n').AsSpan();
        for (int y = 0; y < lines.Length; ++y)
        {
            for (int x = 0; x < lines[y].Length; ++x)
            {
                if (lines[y][x] == '#')
                    elves.Add((x, y));
            }
        }
        return elves;
    }

    protected override int RunPartOne()
    {
        for (int i = 0; i <= 9; ++i)
            PerformRound(i);

        Coord min = (_input.Min(elf => elf.X), _input.Min(elf => elf.Y));
        Coord max = (_input.Max(elf => elf.X), _input.Max(elf => elf.Y));
        return (max.X - min.X + 1) * (max.Y - min.Y + 1) - _input.Count;
    }

    protected override int RunPartTwo()
    {
        int offset = 0;
        while (PerformRound(offset) > 0)
            ++offset;

        return offset + 1;
    }
}
