using Coord = (int X, int Y);

internal record Map(string[] Tiles, Coord Start, Coord End);

internal record Junction(Coord Position, Coord[] Directions);

internal class Puzzle(string rawInput) : AocPuzzle<Map, int>(rawInput)
{
    private static readonly Dictionary<char, Coord> _directions = new()
    {
        ['^'] = (0, -1),
        ['>'] = (1, 0),
        ['v'] = (0, 1),
        ['<'] = (-1, 0)
    };

    private int GetMaxPathLength1()
    {
        int maxLength = 0;

        int CalculateLength(Coord pos, HashSet<Coord> visited, int length)
        {
            visited.Add(pos);
            List<Coord> nextPositions = [];
            while (true)
            {
                foreach (var delta in _directions.Values)
                {
                    Coord nextPos = (pos.X + delta.X, pos.Y + delta.Y);
                    char nextTile = _input.Tiles[nextPos.Y][nextPos.X];
                    if (nextTile != '#' && !visited.Contains(nextPos) && (nextTile == '.' || _directions[nextTile] == delta))
                        nextPositions.Add(nextPos);
                }

                if (nextPositions.Count != 1)
                    break;

                ++length;
                pos = nextPositions[0];
                if (pos == _input.End)
                    return length;

                visited.Add(pos);
                nextPositions.Clear();
            }

            foreach (var nextPos in nextPositions)
            {
                int nextLength = CalculateLength(nextPos, [..visited], length + 1);
                if (nextLength > maxLength)
                    maxLength = nextLength;
            }

            return maxLength;
        }

        return CalculateLength((_input.Start.X, _input.Start.Y + 1), [_input.Start], 1);
    }

    private Junction[] GetJunctions()
    {
        List<Junction> junctions = [new(_input.Start, [(0, 1)])];
        for (int y = 1; y < _input.End.Y; ++y)
        {
            for (int x = 1; x < _input.Tiles[0].Length - 1; ++x)
            {
                if (_input.Tiles[y][x] != '.')
                    continue;

                List<Coord> directions = [];
                foreach (var delta in _directions.Values)
                {
                    Coord pos = (x + delta.X, y + delta.Y);
                    char tile = _input.Tiles[pos.Y][pos.X];
                    if (tile is not ('#' or '.'))
                        directions.Add(delta);
                }

                if (directions.Count > 1)
                    junctions.Add(new((x, y), directions.ToArray()));
            }
        }

        junctions.Add(new(_input.End, [(0, -1)]));
        return junctions.ToArray();
    }

    private Dictionary<Coord, Dictionary<Coord, int>> GetJunctionSteps()
    {
        Dictionary<Coord, Dictionary<Coord, int>> junctionSteps = [];
        var junctions = GetJunctions();
        foreach (var junction in junctions)
            junctionSteps[junction.Position] = [];

        HashSet<Coord> visited = [];
        foreach (var junction in junctions[1..^1])
        {
            visited.Add(junction.Position);
            foreach (var (dirX, dirY) in junction.Directions)
            {
                Coord pos = (junction.Position.X + dirX, junction.Position.Y + dirY);
                visited.Add(pos);
                int steps = 1;
                bool finished = false;
                do
                {
                    bool foundNeighbor = false;
                    foreach (var (deltaX, deltaY) in _directions.Values)
                    {
                        Coord nextPos = (pos.X + deltaX, pos.Y + deltaY);
                        if (nextPos != junction.Position && junctionSteps.TryGetValue(nextPos, out var nextJunctionSteps))
                        {
                            nextJunctionSteps[junction.Position] = steps + 1;
                            junctionSteps[junction.Position][nextPos] = steps + 1;
                            finished = true;
                            break;
                        }

                        if (_input.Tiles[nextPos.Y][nextPos.X] != '#' && !visited.Contains(nextPos))
                        {
                            ++steps;
                            visited.Add(nextPos);
                            pos = nextPos;
                            foundNeighbor = true;
                            break;
                        }
                    }

                    if (!foundNeighbor)
                        break;
                } while (!finished);
            }
        }

        return junctionSteps;
    }

    private int GetMaxPathLength2()
    {
        var junctionSteps = GetJunctionSteps();
        var firstJunction = junctionSteps[_input.Start].First().Key;
        var lastJunction = junctionSteps[_input.End].First().Key;
        int maxLength = 0;

        int CalculateLength(Coord junction, HashSet<Coord> visited, int length)
        {
            if (junction == lastJunction)
                return length;

            foreach (var (nextJunction, steps) in junctionSteps[junction])
            {
                if (!visited.Contains(nextJunction))
                {
                    HashSet<Coord> nextVisited = [..visited, nextJunction];
                    int nextLength = CalculateLength(nextJunction, nextVisited, length + steps);
                    if (nextLength > maxLength)
                        maxLength = nextLength;
                }
            }

            return maxLength;
        }

        return CalculateLength(firstJunction, [_input.Start, firstJunction], junctionSteps[_input.Start][firstJunction]) + junctionSteps[lastJunction][_input.End];
    }


    protected override Map ParseInput(string rawInput)
    {
        var tiles = rawInput.Split('\n');
        int startX = tiles[0].IndexOf('.');
        return new(tiles, (startX, 0), (tiles[^1].IndexOf('.'), tiles.Length - 1));
    }

    protected override int RunPartOne() => GetMaxPathLength1();

    protected override int RunPartTwo() => GetMaxPathLength2();
}
