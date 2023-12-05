internal class Puzzle(string rawInput) : AocPuzzle<Almanac, long>(rawInput)
{
    private static long[] GetNumbers(string list) => list.Split(' ').Select(long.Parse).ToArray();

    private static Map CreateMap(string definition)
    {
        var parts = GetNumbers(definition);
        return new(new(parts[1], parts[1] + parts[2] - 1), new(parts[0], parts[0] + parts[2] - 1));
    }

    private static Map[] CreateMaps(string section) => section.Split('\n')[1..].Select(CreateMap).ToArray();

    private static long MapValue(long value, Map[] maps)
    {
        foreach (var map in maps)
        {
            if (map.Source.Contains(value))
                return map.Dest.Start + value - map.Source.Start;
        }
        return value;
    }

    private static long ReverseMappedValue(long value, Map[] maps)
    {
        foreach (var map in maps)
        {
            if (map.Dest.Contains(value))
                return map.Source.Start + value - map.Dest.Start;
        }
        return value;
    }

    private long GetLocation(long seed)
    {
        long soil = MapValue(seed, _input.SeedMaps);
        long fertilizer = MapValue(soil, _input.SoilMaps);
        long water = MapValue(fertilizer, _input.FertilizerMaps);
        long light = MapValue(water, _input.WaterMaps);
        long temperature = MapValue(light, _input.LightMaps);
        long humidity = MapValue(temperature, _input.TemperatureMaps);
        return MapValue(humidity, _input.HumidityMaps);
    }

    private long GetSeed(long location)
    {
        long humidity = ReverseMappedValue(location, _input.HumidityMaps);
        long temperature = ReverseMappedValue(humidity, _input.TemperatureMaps);
        long light = ReverseMappedValue(temperature, _input.LightMaps);
        long water = ReverseMappedValue(light, _input.WaterMaps);
        long fertilizer = ReverseMappedValue(water, _input.FertilizerMaps);
        long soil = ReverseMappedValue(fertilizer, _input.SoilMaps);
        return ReverseMappedValue(soil, _input.SeedMaps);
    }


    protected override Almanac ParseInput(string rawInput)
    {
        var sections = rawInput.Split("\n\n");
        return new(
            GetNumbers(sections[0].Split(": ")[1]),
            CreateMaps(sections[1]),
            CreateMaps(sections[2]),
            CreateMaps(sections[3]),
            CreateMaps(sections[4]),
            CreateMaps(sections[5]),
            CreateMaps(sections[6]),
            CreateMaps(sections[7]));
    }

    protected override long RunPartOne() => _input.Seeds.Min(GetLocation);

    protected override long RunPartTwo()
    {
        List<Range> seedRanges = [];
        for (int i = 0; i < _input.Seeds.Length; i += 2)
            seedRanges.Add(new(_input.Seeds[i], _input.Seeds[i] + _input.Seeds[i + 1]));

        for (long location = 0; ; ++location)
        {
            long seed = GetSeed(location);
            if (seedRanges.Any(range => range.Contains(seed)))
                return location;
        }
    }
}
