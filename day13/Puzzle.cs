internal class Puzzle(string rawInput) : AocPuzzle<string[][], int>(rawInput)
{
    private static int FindVerticalReflectionLine(string[] pattern, int ignore = 0)
    {
        int max = pattern[0].Length - 1;
        int ignoredX = ignore - 1;
        for (int x = 0; x < max; ++x)
        {
            if (x == ignoredX)
                continue;

            bool reflects = true;
            int maxOffset = Math.Min(x, max - x - 1);
            for (int offset = 0; offset <= maxOffset; ++offset)
            {
                int x1 = x - offset;
                int x2 = x + offset + 1;
                for (int y = 0; y < pattern.Length; ++y)
                {
                    if (pattern[y][x1] != pattern[y][x2])
                    {
                        reflects = false;
                        break;
                    }
                }

                if (!reflects)
                    break;
            }

            if (reflects)
                return x + 1;
        }

        return -1;
    }

    private static int FindHorizontalReflectionLine(string[] pattern, int ignore = 0)
    {
        int max = pattern.Length - 1;
        int ignoredY = ignore - 1;
        for (int y = 0; y < max; ++y)
        {
            if (y == ignoredY)
                continue;

            bool reflects = true;
            int maxOffset = Math.Min(y, max - y - 1);
            for (int offset = 0; offset <= maxOffset; ++offset)
            {
                int y1 = y - offset;
                int y2 = y + offset + 1;
                for (int x = 0; x < pattern[0].Length; ++x)
                {
                    if (pattern[y1][x] != pattern[y2][x])
                    {
                        reflects = false;
                        break;
                    }
                }

                if (!reflects)
                    break;
            }

            if (reflects)
                return y + 1;
        }

        return -1;
    }


    protected override string[][] ParseInput(string rawInput) => rawInput.Split("\n\n").Select(section => section.Split('\n')).ToArray();

    protected override int RunPartOne()
    {
        int result = 0;
        foreach (var section in _input)
        {
            int line = FindVerticalReflectionLine(section);
            if (line != -1)
                result += line;
            else
            {
                line = FindHorizontalReflectionLine(section);
                if (line != -1)
                    result += 100 * line;
            }
        }

        return result;
    }

    protected override int RunPartTwo()
    {
        int result = 0;
        foreach (var section in _input)
        {
            int vertLine = FindVerticalReflectionLine(section);
            int horizLine = FindHorizontalReflectionLine(section);
            bool found = false;
            for (int y = 0; y < section.Length; ++y)
            {
                for (int x = 0; x < section[0].Length; ++x)
                {
                    string newLine = section[y][..x] + (section[y][x] == '.' ? '#' : '.') + section[y][(x + 1)..];
                    string[] newSection = [..section[..y], newLine, ..section[(y + 1)..]];
                    int line = FindVerticalReflectionLine(newSection, vertLine);
                    if (line != -1)
                    {
                        result += line;
                        found = true;
                        break;
                    }
                    else
                    {
                        line = FindHorizontalReflectionLine(newSection, horizLine);
                        if (line != -1)
                        {
                            result += 100 * line;
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                    break;
            }
        }

        return result;
    }
}
