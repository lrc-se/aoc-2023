using Node = (string Name, string Left, string Right);

internal record Map(char[] LeftRight, Dictionary<string, Node> Nodes);

internal class Puzzle(string rawInput) : AocPuzzle<Map, long>(rawInput)
{
    private static Node CreateNode(string definition)
    {
        var parts = definition.Split(" = ");
        var dirParts = parts[1].Split(", ");
        return (parts[0], dirParts[0][1..], dirParts[1][..^1]);
    }

    private static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    private static long Lcm(long a, long b) => a * b / Gcd(a, b);

    private long GetSteps(Node startNode, Func<string, bool> endCondition)
    {
        long steps = 0;
        var curNode = startNode;
        do
        {
            curNode = _input.Nodes[_input.LeftRight[steps % _input.LeftRight.Length] == 'L' ? curNode.Left : curNode.Right];
            ++steps;
        } while (!endCondition(curNode.Name));
        return steps;
    }


    protected override Map ParseInput(string rawInput)
    {
        var sections = rawInput.Split("\n\n");
        var nodes = new Dictionary<string, Node>();
        foreach (string line in sections[1].Split('\n'))
        {
            var node = CreateNode(line);
            nodes.Add(node.Name, node);
        }
        return new(sections[0].ToCharArray(), nodes);
    }

    protected override long RunPartOne() => GetSteps(_input.Nodes["AAA"], name => name == "ZZZ");

    protected override long RunPartTwo()
        => _input.Nodes.Values
            .Where(node => node.Name.EndsWith('A'))
            .Select(node => GetSteps(node, name => name.EndsWith('Z')))
            .Aggregate(Lcm);
}
