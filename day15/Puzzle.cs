internal class Puzzle(string rawInput) : AocPuzzle<string[], long>(rawInput)
{
    private static int GetHash(string value)
    {
        int hash = 0;
        for (int i = 0; i < value.Length; ++i)
        {
            hash += value[i];
            hash *= 17;
            hash %= 256;
        }
        return hash;
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split(',', '\n').ToArray();

    protected override long RunPartOne() => _input.Select(GetHash).Sum();

    protected override long RunPartTwo()
    {
        var boxes = new Box[256];
        for (int i = 0; i < boxes.Length; ++i)
            boxes[i] = new();

        foreach (string step in _input)
        {
            if (step[^1] == '-')
            {
                string label = step[..^1];
                boxes[GetHash(label)].RemoveLens(label);
            }
            else
            {
                string label = step[..^2];
                boxes[GetHash(label)].AddLens((label, step[^1] - 48));
            }
        }

        return boxes.Select((box, boxNum) => (boxNum + 1) * box.GetFocalPower()).Sum();
    }
}
