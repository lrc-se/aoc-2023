global using Coord = (int X, int Y);

internal readonly record struct SchematicObject<T>(Coord Position, T Value, int Width)
{
    public bool IsAdjacentTo<TOther>(SchematicObject<TOther> other)
        => other.Position.Y >= Position.Y - 1
            && other.Position.Y <= Position.Y + 1
            && other.Position.X + other.Width >= Position.X
            && other.Position.X <= Position.X + Width;
}

internal record EngineSchematic(SchematicObject<int>[] Numbers, SchematicObject<char>[] Symbols);
