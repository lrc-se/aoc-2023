using Counts = (int Red, int Green, int Blue);

internal class Game
{
    public int Id { get; set; }
    public Counts[] Revelations { get; set; }

    public Game(string definition)
    {
        var parts = definition.Split(": ");
        Id = int.Parse(parts[0][(parts[0].IndexOf(' ') + 1)..]);

        parts = parts[1].Split("; ");
        List<Counts> revelations = [];
        foreach (string part in parts)
        {
            var colors = part.Split(", ");
            int redCount = 0;
            int greenCount = 0;
            int blueCount = 0;
            foreach (string color in colors)
            {
                var colorParts = color.Split(' ');
                int count = int.Parse(colorParts[0]);
                if (colorParts[1] == "red")
                    redCount = count;
                else if (colorParts[1] == "green")
                    greenCount = count;
                else if (colorParts[1] == "blue")
                    blueCount = count;
            }
            revelations.Add((redCount, greenCount, blueCount));
        }
        Revelations = revelations.ToArray();
    }
}
