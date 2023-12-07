internal class Puzzle(string rawInput) : AocPuzzle<Hand[], int>(rawInput)
{
    private static Hand CreateHand(string definition)
    {
        var parts = definition.Split(' ');
        return new(parts[0].ToCharArray().Select(label => new Card(label)).ToArray(), int.Parse(parts[1]));
    }


    protected override Hand[] ParseInput(string rawInput) => rawInput.Split('\n').Select(CreateHand).ToArray();

    protected override int RunPartOne()
    {
        Array.Sort(_input);
        int winnings = 0;
        for (int i = 0; i < _input.Length; ++i)
            winnings += _input[i].Bid * (i + 1);

        return winnings;
    }

    protected override int RunPartTwo() => 0;
}
