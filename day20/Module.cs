global using ModuleOutput = (string FromModuleName, string ToModuleName, Pulse Pulse);

internal enum Pulse { Low, High }

internal abstract class Module(string name, string[] outputs)
{
    public readonly string Name = name;
    public readonly string[] Outputs = outputs;

    public abstract ModuleOutput[] HandlePulse(string fromModule, Pulse pulse);
}

internal class FlipFlopModule(string name, string[] outputs) : Module(name, outputs)
{
    private bool _isOn = false;

    public override ModuleOutput[] HandlePulse(string _, Pulse pulse)
    {
        if (pulse == Pulse.High)
            return [];

        _isOn = !_isOn;
        var nextPulse = _isOn ? Pulse.High : Pulse.Low;
        return Outputs.Select(name => (Name, name, nextPulse)).ToArray();
    }
}

internal class ConjunctionModule(string name, string[] outputs) : Module(name, outputs)
{
    private readonly Dictionary<string, Pulse> _inputValues = [];

    public int InputCount => _inputValues.Count;

    public override ModuleOutput[] HandlePulse(string fromModule, Pulse pulse)
    {
        _inputValues[fromModule] = pulse;
        if (pulse == Pulse.High)
            OnHighInput?.Invoke(fromModule);

        var nextPulse = _inputValues.Values.All(p => p == Pulse.High) ? Pulse.Low : Pulse.High;
        return Outputs.Select(name => (Name, name, nextPulse)).ToArray();
    }

    public void AddInput(string moduleName) => _inputValues[moduleName] = Pulse.Low;

    public event Action<string>? OnHighInput;
}

internal class BroadcastModule(string name, string[] outputs) : Module(name, outputs)
{
    public override ModuleOutput[] HandlePulse(string _, Pulse pulse) => Outputs.Select(name => (Name, name, pulse)).ToArray();
}
