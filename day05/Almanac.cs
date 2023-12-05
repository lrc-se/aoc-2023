internal readonly record struct Range(long Start, long End)
{
    public bool Contains(long value) => value >= Start && value <= End;
}

internal record Map(Range Source, Range Dest);

internal record Almanac(long[] Seeds, Map[] SeedMaps, Map[] SoilMaps, Map[] FertilizerMaps, Map[] WaterMaps, Map[] LightMaps, Map[] TemperatureMaps, Map[] HumidityMaps);
