using System.Text.RegularExpressions;

internal class Puzzle(string rawInput) : AocPuzzle<Card[], int>(rawInput)
{
    private static readonly Regex _re = new(@"^Card\s+(\d+): (.+) \| (.+)$", RegexOptions.Compiled);
    private static readonly Regex _re2 = new(@"\s+", RegexOptions.Compiled);

    private static Card CreateCard(string line)
    {
        var match = _re.Match(line);
        return new(int.Parse(match.Groups[1].Value), GetNumbers(match.Groups[2].Value), GetNumbers(match.Groups[3].Value));
    }

    private static int[] GetNumbers(string source) => _re2.Split(source.Trim()).Select(int.Parse).ToArray();


    protected override Card[] ParseInput(string rawInput) => rawInput.Split('\n').Select(CreateCard).ToArray();

    protected override int RunPartOne() => _input.Select(card => card.GetPoints()).Sum();

    protected override int RunPartTwo()
    {
        var cardCounts = _input.ToDictionary(card => card.Id, _ => 1);
        foreach (var card in _input)
        {
            int matchCount = card.GetMatchCount();
            if (matchCount > 0)
            {
                int max = Math.Min(card.Id + matchCount, _input.Length);
                for (int num = card.Id + 1; num <= max; ++num)
                    cardCounts[num] += cardCounts[card.Id];
            }
        }

        return cardCounts.Values.Sum();
    }
}
