internal class Puzzle(string rawInput) : AocPuzzle<Game[], int>(rawInput)
{
    protected override Game[] ParseInput(string rawInput) => rawInput.Split('\n').Select(line => new Game(line)).ToArray();

    protected override int RunPartOne()
        => _input
            .Where(game => game.Revelations.All(rev => rev.Red <= 12 && rev.Green <= 13 && rev.Blue <= 14))
            .Sum(game => game.Id);

    protected override int RunPartTwo()
        => _input
            .Select(game => game.Revelations.Max(rev => rev.Red) * game.Revelations.Max(rev => rev.Green) * game.Revelations.Max(rev => rev.Blue))
            .Sum();
}
