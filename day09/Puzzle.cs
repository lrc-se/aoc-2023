internal class Puzzle(string rawInput) : AocPuzzle<int[][], int>(rawInput)
{
    private static int[] FindBoundaryValues(int[] values, Index boundary)
    {
        List<int> boundaryValues = [values[boundary]];
        var prevValues = values;
        bool end;
        do
        {
            end = true;
            int max = prevValues.Length - 1;
            var curValues = new int[max];
            for (int i = 0; i < max; ++i)
            {
                int value = prevValues[i + 1] - prevValues[i];
                curValues[i] = value;
                if (value != 0)
                    end = false;
            }

            boundaryValues.Add(curValues[boundary]);
            prevValues = curValues;
        } while (!end);

        return boundaryValues.ToArray();
    }

    private static int FindNextValue(int[] values) => FindBoundaryValues(values, ^1).Sum();

    private static int FindPrevValue(int[] values)
    {
        var firstValues = FindBoundaryValues(values, 0);
        int value = firstValues[^2];
        for (int i = firstValues.Length - 3; i >= 0; --i)
            value = firstValues[i] - value;

        return value;
    }


    protected override int[][] ParseInput(string rawInput)
        => rawInput
            .Split('\n')
            .Select(line => line.Split(' ').Select(int.Parse).ToArray())
            .ToArray();

    protected override int RunPartOne() => _input.Select(FindNextValue).Sum();

    protected override int RunPartTwo() => _input.Select(FindPrevValue).Sum();
}
