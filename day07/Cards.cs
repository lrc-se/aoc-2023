internal enum HandType { HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind }

internal readonly struct Card(char label)
{
    public char Label { get; } = label;
    public int Strength { get; } = label switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        _ => label - 48
    };
}

internal readonly struct Hand(Card[] cards, int bid) : IComparable<Hand>
{
    public HandType Type { get; } = ComputeType(cards);
    public Card[] Cards { get; } = cards;
    public int Bid { get; } = bid;

    private static HandType ComputeType(Card[] cards)
    {
        var sortedCards = cards.OrderBy(card => card.Label).ToArray();
        List<int> runCounts = [];
        int runCount = 1;
        for (int i = 1; i < sortedCards.Length; ++i)
        {
            if (sortedCards[i].Label == sortedCards[i - 1].Label)
            {
                ++runCount;
            }
            else if (runCount > 1)
            {
                runCounts.Add(runCount);
                runCount = 1;
            }
        }
        if (runCount > 1)
            runCounts.Add(runCount);

        runCounts.Sort();

        return runCounts switch
        {
            [2] => HandType.OnePair,
            [2, 2] => HandType.TwoPair,
            [3] => HandType.ThreeOfAKind,
            [2, 3] => HandType.FullHouse,
            [4] => HandType.FourOfAKind,
            [5] => HandType.FiveOfAKind,
            _ => HandType.HighCard
        };
    }

    public int CompareTo(Hand other)
    {
        int diff = Type - other.Type;
        if (diff != 0)
            return diff;

        for (int i = 0; i < Cards.Length; ++i)
        {
            diff = Cards[i].Strength - other.Cards[i].Strength;
            if (diff != 0)
                return diff;
        }

        return 0;
    }
}
