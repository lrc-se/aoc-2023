using Coord = (int X, int Y);

internal class Image
{
    public Coord[] Galaxies { get; }

    private readonly int[] _rowSizes;
    private readonly int[] _colSizes;

    public Image(string[] data, int expansionFactor)
    {
        _rowSizes = new int[data.Length];
        _colSizes = new int[data[0].Length];
        Array.Fill(_colSizes, expansionFactor);

        List<Coord> galaxies = [];
        for (int y = 0; y < _rowSizes.Length; ++y)
        {
            bool isExpandedRow = true;
            for (int x = 0; x < _colSizes.Length; ++x)
            {
                if (data[y][x] == '#')
                {
                    galaxies.Add((x, y));
                    isExpandedRow = false;
                    _colSizes[x] = 1;
                }
            }
            _rowSizes[y] = isExpandedRow ? expansionFactor : 1;
        }

        Galaxies = galaxies.ToArray();
    }

    public long GetDistance(Coord source, Coord dest)
    {
        (int minX, int maxX) = source.X <= dest.X ? (source.X, dest.X) : (dest.X, source.X);
        (int minY, int maxY) = source.Y <= dest.Y ? (source.Y, dest.Y) : (dest.Y, source.Y);
        return _colSizes[minX..maxX].Sum() + _rowSizes[minY..maxY].Sum();
    }
}
