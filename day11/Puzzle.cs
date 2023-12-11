internal class Puzzle(string rawInput) : AocPuzzle<string[], long>(rawInput)
{
    private long RunPuzzle(int expansionFactor)
    {
        var image = new Image(_input, expansionFactor);
        long result = 0;
        for (int i = 0; i < image.Galaxies.Length - 1; ++i)
        {
            for (int j = i + 1; j < image.Galaxies.Length; ++j)
            {
                result += image.GetDistance(image.Galaxies[i], image.Galaxies[j]);
            }
        }
        return result;
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override long RunPartOne() => RunPuzzle(2);

    protected override long RunPartTwo() => RunPuzzle(1000000);
}
