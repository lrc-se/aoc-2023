internal enum HandType { HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind }

internal readonly struct Card(char label, bool useJokers)
{
    public readonly char Label = label;
    public readonly int Strength = label switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => useJokers ? 1 : 11,
        'T' => 10,
        _ => label - 48
    };
}

internal readonly struct Hand(Card[] cards, int bid, bool useJokers) : IComparable<Hand>
{
    public readonly HandType Type = useJokers ? ComputeTypeWithJokers(cards) : ComputeType(cards.Select(card => card.Label).OrderBy(label => label).ToArray());
    public readonly Card[] Cards = cards;
    public readonly int Bid = bid;

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

    private static readonly char[] _jokerLabels = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];

    private static HandType ComputeType(ReadOnlySpan<char> sortedLabels)
    {
        List<int> runCounts = [];
        int runCount = 1;
        for (int i = 1; i < sortedLabels.Length; ++i)
        {
            if (sortedLabels[i] == sortedLabels[i - 1])
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

        return runCounts switch
        {
            [2] => HandType.OnePair,
            [2, 2] => HandType.TwoPair,
            [3] => HandType.ThreeOfAKind,
            [2, 3] or [3, 2] => HandType.FullHouse,
            [4] => HandType.FourOfAKind,
            [5] => HandType.FiveOfAKind,
            _ => HandType.HighCard
        };
    }

    private static HandType ComputeTypeWithJokers(Card[] cards)
    {
        var bestType = HandType.HighCard;
        var cache = new Dictionary<string, HandType>();
        var cardLabels = cards.Select(card => card.Label == 'J' ? _jokerLabels : [card.Label]).ToArray();
        for (int i0 = 0; i0 < cardLabels[0].Length; ++i0)
        {
            for (int i1 = 0; i1 < cardLabels[1].Length; ++i1)
            {
                for (int i2 = 0; i2 < cardLabels[2].Length; ++i2)
                {
                    for (int i3 = 0; i3 < cardLabels[3].Length; ++i3)
                    {
                        for (int i4 = 0; i4 < cardLabels[4].Length; ++i4)
                        {
                            var labels = new[] { cardLabels[0][i0], cardLabels[1][i1], cardLabels[2][i2], cardLabels[3][i3], cardLabels[4][i4] }.AsSpan();
                            labels.Sort();

                            string key = labels.ToString();
                            if (!cache.TryGetValue(key, out var type))
                            {
                                type = ComputeType(labels);
                                cache[key] = type;
                            }

                            if (type > bestType)
                                bestType = type;
                        }
                    }
                }
            }
        }
        return bestType;
    }
}
