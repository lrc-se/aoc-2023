using Lens = (string Label, int FocalLength);

internal class Box
{
    private readonly List<Lens> _lenses = [];
    private readonly Dictionary<string, int> _lensIndex = [];

    public void AddLens(Lens lens)
    {
        if (_lensIndex.TryGetValue(lens.Label, out int index))
            _lenses[index] = lens;
        else
        {
            _lensIndex[lens.Label] = _lenses.Count;
            _lenses.Add(lens);
        }
    }

    public void RemoveLens(string label)
    {
        if (_lensIndex.TryGetValue(label, out int index))
        {
            _lenses.RemoveAt(index);
            _lensIndex.Remove(label);
            for (int i = index; i < _lenses.Count; ++i)
                --_lensIndex[_lenses[i].Label];
        }
    }

    public int GetFocalPower()
    {
        int power = 0;
        for (int i = 0; i < _lenses.Count; ++i)
            power += (i + 1) * _lenses[i].FocalLength;

        return power;
    }
}
