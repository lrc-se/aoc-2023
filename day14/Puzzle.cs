internal class Puzzle(string rawInput) : AocPuzzle<char[][], int>(rawInput)
{
    private void TiltNorth()
    {
        for (int y = 1; y < _input.Length; ++y)
        {
            for (int x = 0; x < _input[0].Length; ++x)
            {
                if (_input[y][x] != 'O' || _input[y - 1][x] != '.')
                    continue;

                _input[y][x] = '.';
                for (int y2 = y - 1; y2 >= 0; --y2)
                {
                    if (y2 == 0 || _input[y2 - 1][x] != '.')
                    {
                        _input[y2][x] = 'O';
                        break;
                    }
                }
            }
        }
    }

    private void TiltWest()
    {
        for (int x = 1; x < _input[0].Length; ++x)
        {
            for (int y = 0; y < _input.Length; ++y)
            {
                if (_input[y][x] != 'O' || _input[y][x - 1] != '.')
                    continue;

                _input[y][x] = '.';
                for (int x2 = x - 1; x2 >= 0; --x2)
                {
                    if (x2 == 0 || _input[y][x2 - 1] != '.')
                    {
                        _input[y][x2] = 'O';
                        break;
                    }
                }
            }
        }
    }

    private void TiltSouth()
    {
        for (int y = _input.Length - 2; y >= 0; --y)
        {
            for (int x = 0; x < _input[0].Length; ++x)
            {
                if (_input[y][x] != 'O' || _input[y + 1][x] != '.')
                    continue;

                _input[y][x] = '.';
                for (int y2 = y + 1; y2 < _input.Length; ++y2)
                {
                    if (y2 == _input.Length - 1 || _input[y2 + 1][x] != '.')
                    {
                        _input[y2][x] = 'O';
                        break;
                    }
                }
            }
        }
    }

    private int TiltEast()
    {
        int checksum = 0;
        for (int x = _input[0].Length - 2; x >= 0; --x)
        {
            for (int y = 0; y < _input.Length; ++y)
            {
                if (_input[y][x] != 'O' || _input[y][x + 1] != '.')
                    continue;

                _input[y][x] = '.';
                for (int x2 = x + 1; x2 < _input[0].Length; ++x2)
                {
                    if (x2 == _input[0].Length - 1 || _input[y][x2 + 1] != '.')
                    {
                        _input[y][x2] = 'O';
                        checksum += y * _input[0].Length + x2;
                        break;
                    }
                }
            }
        }

        return checksum;
    }

    private int PerformTiltCycle()
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        return TiltEast();
    }

    private int GetLoad()
    {
        int load = 0;
        for (int y = 0; y < _input.Length; ++y)
            load += _input[y].AsSpan().Count('O') * (_input.Length - y);

        return load;
    }


    protected override char[][] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => line.ToCharArray()).ToArray();

    protected override int RunPartOne()
    {
        TiltNorth();
        return GetLoad();
    }

    protected override int RunPartTwo()
    {
        int cycleOffset;
        int cycle = 0;
        var checksums = new Dictionary<int, int>();
        while (true)
        {
            int checksum = PerformTiltCycle();
            if (checksums.TryGetValue(checksum, out cycleOffset))
                break;

            checksums[checksum] = cycle++;
        }

        int remainingCycles = (1000000000 - cycleOffset - 1) % (cycle - cycleOffset);
        for (int i = 0; i < remainingCycles; ++i)
            PerformTiltCycle();

        return GetLoad();
    }
}
