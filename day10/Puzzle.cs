using Coord = (int X, int Y);

internal enum Direction { North, East, South, West }

internal class Puzzle(string rawInput) : AocPuzzle<char[][], int>(rawInput)
{
    private static readonly Dictionary<Direction, Coord> _deltas = new()
    {
        [Direction.North] = (0, -1),
        [Direction.East] = (1, 0),
        [Direction.South] = (0, 1),
        [Direction.West] = (-1, 0)
    };

    private (Coord Position, Direction Direction, char Tile) GetStart()
    {
        int x;
        int y = -1;
        do
        {
            x = _input[++y].AsSpan().IndexOf('S');
        } while (x == -1);

        Coord start = (x, y);
        bool hasNorth = y > 0 && _input[y - 1][x] is '|' or '7' or 'F';
        bool hasEast = x < _input[0].Length - 1 && _input[y][x + 1] is '-' or 'J' or '7';
        bool hasSouth = y < _input.Length - 1 && _input[y + 1][x] is '|' or 'L' or 'J';

        return (hasNorth, hasEast, hasSouth) switch
        {
            (true, true, false) => (start, Direction.North, 'L'),
            (true, false, true) => (start, Direction.North, '|'),
            (true, false, false) => (start, Direction.North, 'J'),
            (false, true, true) => (start, Direction.East, 'F'),
            (false, true, false) => (start, Direction.East, '-'),
            _ => (start, Direction.South, '7')
        };
    }

    private Direction GetNextDirection(Coord pos, Direction dir) => _input[pos.Y][pos.X] switch
    {
        '|' => dir == Direction.North ? Direction.North : Direction.South,
        '-' => dir == Direction.East ? Direction.East : Direction.West,
        'L' => dir == Direction.South ? Direction.East : Direction.North,
        'J' => dir == Direction.South ? Direction.West : Direction.North,
        '7' => dir == Direction.North ? Direction.West : Direction.South,
        'F' => dir == Direction.North ? Direction.East : Direction.South,
        _ => dir
    };


    protected override char[][] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => line.ToCharArray()).ToArray();

    protected override int RunPartOne()
    {
        var (start, curDir, _) = GetStart();

        int steps = 0;
        var curPos = start;
        do
        {
            curDir = GetNextDirection(curPos, curDir);
            var (deltaX, deltaY) = _deltas[curDir];
            curPos = (curPos.X + deltaX, curPos.Y + deltaY);
            ++steps;
        } while (curPos != start);

        return steps / 2;
    }

    protected override int RunPartTwo()
    {
        var (startPos, curDir, startTile) = GetStart();
        _input[startPos.Y][startPos.X] = startTile;

        HashSet<Coord> loopTiles = [];
        var curPos = startPos;
        do
        {
            loopTiles.Add(curPos);
            curDir = GetNextDirection(curPos, curDir);
            var (deltaX, deltaY) = _deltas[curDir];
            curPos = (curPos.X + deltaX, curPos.Y + deltaY);
        } while (curPos != startPos);

        List<Coord> groundTiles = [];
        int expandedWidth = _input[0].Length * 2 + 1;
        var expandedGrid = new List<char[]>(_input.Length * 2 + 1) { new char[expandedWidth] };
        Array.Fill(expandedGrid[0], '.');
        for (int y = 0; y < _input.Length; ++y)
        {
            int y2 = y * 2 + 1;
            expandedGrid.Add(new char[expandedWidth]);
            expandedGrid.Add(new char[expandedWidth]);
            expandedGrid[y2][0] = '.';
            expandedGrid[y2 + 1][0] = '.';

            for (int x = 0; x < _input[0].Length; ++x)
            {
                char tile = _input[y][x];
                char xTile = '.';
                char yTile = '.';
                if (loopTiles.Contains((x, y)))
                {
                    xTile = tile is not ('|' or 'J' or '7') ? '-' : '.';
                    yTile = tile is not ('-' or 'J' or 'L') ? '|' : '.';
                }
                else
                {
                    groundTiles.Add((x, y));
                    tile = '.';
                }

                int x2 = x * 2 + 1;
                expandedGrid[y2][x2] = tile;
                expandedGrid[y2][x2 + 1] = xTile;
                expandedGrid[y2 + 1][x2] = yTile;
                expandedGrid[y2 + 1][x2 + 1] = '.';
            }
        }

        var coords = new Queue<Coord>([(0, 0)]);
        while (coords.Count > 0)
        {
            var (x, y) = coords.Dequeue();
            if (expandedGrid[y][x] == '.')
            {
                expandedGrid[y][x] = 'O';

                if (y > 0)
                    coords.Enqueue((x, y - 1));

                if (x < expandedGrid[0].Length - 1)
                    coords.Enqueue((x + 1, y));

                if (y < expandedGrid.Count - 1)
                    coords.Enqueue((x, y + 1));

                if (x > 0)
                    coords.Enqueue((x - 1, y));
            }
        }

        return groundTiles.Count(coord => expandedGrid[coord.Y * 2 + 1][coord.X * 2 + 1] != 'O');
    }
}
