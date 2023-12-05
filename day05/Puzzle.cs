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

    private static Range[] MapRanges(Range source, Map[] maps)
    {
        List<Range> ranges = [];
        long curStart = source.Start;

        var sortedMaps = maps.Where(map => map.Source.Start <= source.End && map.Source.End >= source.Start).OrderBy(map => map.Source.Start);
        foreach (var map in sortedMaps)
        {
            if (curStart < map.Source.Start)
                ranges.Add(new(curStart, map.Source.Start - 1));

            long start = Math.Max(map.Source.Start, source.Start);
            long end = Math.Min(map.Source.End, source.End);
            ranges.Add(new(map.Dest.Start + start - map.Source.Start, map.Dest.Start + end - map.Source.Start));
            curStart = end + 1;
        }

        if (curStart < source.End)
            ranges.Add(new(curStart, source.End));

        return ranges.ToArray();
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

        var soilRanges = seedRanges.SelectMany(range => MapRanges(range, _input.SeedMaps));
        var fertilizerRanges = soilRanges.SelectMany(range => MapRanges(range, _input.SoilMaps));
        var waterRanges = fertilizerRanges.SelectMany(range => MapRanges(range, _input.FertilizerMaps));
        var lightRanges = waterRanges.SelectMany(range => MapRanges(range, _input.WaterMaps));
        var temperatureRanges = lightRanges.SelectMany(range => MapRanges(range, _input.LightMaps));
        var humidityRanges = temperatureRanges.SelectMany(range => MapRanges(range, _input.TemperatureMaps));
        var locationRanges = humidityRanges.SelectMany(range => MapRanges(range, _input.HumidityMaps));
        return locationRanges.Min(range => range.Start);
    }
}
