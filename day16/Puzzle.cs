using System.Collections.Frozen;
using Coord = (int X, int Y);

internal enum Direction { Up, Right, Down, Left }

internal class Beam(Coord coord, Direction direction)
{
    public Coord Coord = coord;
    public Direction Direction = direction;
}

internal class Puzzle(string rawInput) : AocPuzzle<string[], int>(rawInput)
{
    private static readonly FrozenDictionary<Direction, Coord> _deltas = new Dictionary<Direction, Coord>
    {
        [Direction.Up] = (0, -1),
        [Direction.Right] = (1, 0),
        [Direction.Down] = (0, 1),
        [Direction.Left] = (-1, 0)
    }.ToFrozenDictionary();

    private int CountEnergizedTiles(Beam startBeam)
    {
        Beam[] beams = [startBeam];
        List<Beam> nextBeams = [];
        HashSet<Coord> energizedTiles = [];
        HashSet<Coord> hitSplitters = [];
        do
        {
            nextBeams.Clear();
            foreach (var beam in beams)
            {
                bool keepBeam = true;
                var (deltaX, deltaY) = _deltas[beam.Direction];
                Coord coord = (beam.Coord.X + deltaX, beam.Coord.Y + deltaY);
                if (coord.X < 0 || coord.X == _input[0].Length || coord.Y < 0 || coord.Y == _input.Length)
                    continue;

                beam.Coord = coord;
                energizedTiles.Add(coord);

                char tile = _input[coord.Y][coord.X];
                if (tile == '\\')
                {
                    beam.Direction = beam.Direction switch
                    {
                        Direction.Up => Direction.Left,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Right,
                        _ => Direction.Up
                    };
                }
                else if (tile == '/')
                {
                    beam.Direction = beam.Direction switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Up,
                        Direction.Down => Direction.Left,
                        _ => Direction.Down
                    };
                }
                else if (tile is '-')
                {
                    if (hitSplitters.Contains(coord))
                        keepBeam = false;
                    else if (beam.Direction is Direction.Up or Direction.Down)
                    {
                        hitSplitters.Add(coord);
                        beam.Direction = Direction.Left;
                        nextBeams.Add(new(coord, Direction.Right));
                    }
                }
                else if (tile == '|')
                {
                    if (hitSplitters.Contains(coord))
                        keepBeam = false;
                    else if (beam.Direction is Direction.Left or Direction.Right)
                    {
                        hitSplitters.Add(coord);
                        beam.Direction = Direction.Up;
                        nextBeams.Add(new(coord, Direction.Down));
                    }
                }

                if (keepBeam)
                    nextBeams.Add(beam);
            }

            beams = nextBeams.ToArray();
        } while (beams.Length > 0);

        return energizedTiles.Count;
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override int RunPartOne() => CountEnergizedTiles(new((-1, 0), Direction.Right));

    protected override int RunPartTwo()
    {
        int max = 0;

        for (int x = 0; x < _input[0].Length; ++x)
        {
            var beam = new Beam((x, -1), Direction.Down);
            int count = CountEnergizedTiles(beam);
            if (count > max)
                max = count;

            beam = new Beam((x, _input.Length), Direction.Up);
            count = CountEnergizedTiles(beam);
            if (count > max)
                max = count;
        }

        for (int y = 0; y < _input.Length; ++y)
        {
            var beam = new Beam((-1, y), Direction.Right);
            int count = CountEnergizedTiles(beam);
            if (count > max)
                max = count;

            beam = new Beam((_input[0].Length, y), Direction.Left);
            count = CountEnergizedTiles(beam);
            if (count > max)
                max = count;
        }

        return max;
    }
}
