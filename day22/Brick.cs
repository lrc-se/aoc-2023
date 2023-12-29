using Coord = (int X, int Y, int Z);

internal class Brick(Coord start, Coord end)
{
    public readonly int MinX = Math.Min(start.X, end.X);
    public readonly int MaxX = Math.Max(start.X, end.X);
    public readonly int MinY = Math.Min(start.Y, end.Y);
    public readonly int MaxY = Math.Max(start.Y, end.Y);
    public int MinZ = Math.Min(start.Z, end.Z);
    public int MaxZ = Math.Max(start.Z, end.Z);

    public bool RestsOn(Brick other) => MinZ == other.MaxZ + 1 && other.MinX <= MaxX && other.MaxX >= MinX && other.MinY <= MaxY && other.MaxY >= MinY;

    public void Fall()
    {
        --MinZ;
        --MaxZ;
    }
}
