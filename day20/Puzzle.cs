internal class Puzzle(string rawInput) : AocPuzzle<Dictionary<string, Module>, long>(rawInput)
{
    private static Module CreateModule(string definition)
    {
        var parts = definition.Split(" -> ");
        var outputs = parts[1].Split(", ");
        return parts[0][0] switch
        {
            '%' => new FlipFlopModule(parts[0][1..], outputs),
            '&' => new ConjunctionModule(parts[0][1..], outputs),
            _ => new BroadcastModule(parts[0], outputs)
        };
    }

    private (long LowPulseCount, long HighPulseCount) PushButton()
    {
        List<ModuleOutput> outputs = [.._input["broadcaster"].HandlePulse("button", Pulse.Low)];
        long lowPulseCount = outputs.Count + 1;
        long highPulseCount = 0;
        do
        {
            List<ModuleOutput> nextOutputs = [];
            foreach (var output in outputs)
            {
                if (!_input.TryGetValue(output.ToModuleName, out var outputModule))
                    continue;

                var curOutputs = outputModule.HandlePulse(output.FromModuleName, output.Pulse);
                long lowCount = curOutputs.LongCount(output => output.Pulse == Pulse.Low);
                lowPulseCount += lowCount;
                highPulseCount += curOutputs.LongLength - lowCount;
                nextOutputs.AddRange(curOutputs);
            }
            outputs = nextOutputs;
        } while (outputs.Count > 0);

        return (lowPulseCount, highPulseCount);
    }

    private static long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    private static long Lcm(long a, long b) => a * b / Gcd(a, b);


    protected override Dictionary<string, Module> ParseInput(string rawInput)
    {
        var modules = rawInput.Split('\n').Select(CreateModule).ToDictionary(module => module.Name);
        foreach (var module in modules.Values)
        {
            foreach (string moduleName in module.Outputs)
            {
                if (modules.TryGetValue(moduleName, out var outputModule) && outputModule is ConjunctionModule conjunctionModule)
                    conjunctionModule.AddInput(module.Name);
            }
        }
        return modules;
    }

    protected override long RunPartOne()
    {
        long lowPulseCount = 0;
        long highPulseCount = 0;
        for (int i = 0; i < 1000; ++i)
        {
            var counts = PushButton();
            lowPulseCount += counts.LowPulseCount;
            highPulseCount += counts.HighPulseCount;
        }

        return lowPulseCount * highPulseCount;
    }

    protected override long RunPartTwo()
    {
        long i = 1;
        Dictionary<string, long> periods = [];
        var penultimateModule = (ConjunctionModule)_input.Values.First(module => module.Outputs.Contains("rx"));
        penultimateModule.OnHighInput += name =>
        {
            if (!periods.ContainsKey(name))
                periods[name] = i;
        };

        for ( ; periods.Count < penultimateModule.InputCount; ++i)
            PushButton();

        return periods.Values.Aggregate(Lcm);
    }
}
