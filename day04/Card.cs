internal record Card(int Id, int[] WinningNumbers, int[] OwnNumbers)
{
    public int GetMatchCount() => OwnNumbers.Count(WinningNumbers.Contains);

    public int GetPoints()
    {
        int count = GetMatchCount();
        return count > 0 ? 1 << (count - 1) : 0;
    }
}
