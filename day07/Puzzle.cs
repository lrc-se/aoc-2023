internal class Puzzle(string rawInput) : AocPuzzle<string[], int>(rawInput)
{
    private static Hand CreateHand(string definition, bool useJokers)
    {
        var parts = definition.Split(' ');
        return new(parts[0].ToCharArray().Select(label => new Card(label, useJokers)).ToArray(), int.Parse(parts[1]), useJokers);
    }

    private int GetWinnings(bool useJokers)
    {
        var hands = _input.Select(line => CreateHand(line, useJokers)).ToArray();
        Array.Sort(hands);

        int winnings = 0;
        for (int i = 0; i < hands.Length; ++i)
            winnings += hands[i].Bid * (i + 1);

        return winnings;
    }


    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override int RunPartOne() => GetWinnings(false);

    protected override int RunPartTwo() => GetWinnings(true);
}
