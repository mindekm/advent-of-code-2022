using System.Diagnostics;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines(@"input.txt");

//Console.WriteLine(Run(lines, 10));
Console.WriteLine(Run(lines, 2_000_000));

int Run(string[] lines, int targetY)
{
    var beaconsAndSensors = new HashSet<Coordinate>();
    var result = new HashSet<Coordinate>();

    foreach (var line in lines)
    {
        var matches = Regex.Matches(line, @"[xy]=([-\d]+)");

        var sensor = new Coordinate(Parse(matches[0]), Parse(matches[1]));
        beaconsAndSensors.Add(sensor);

        var beacon = new Coordinate(Parse(matches[2]), Parse(matches[3]));
        beaconsAndSensors.Add(beacon);

        // https://en.wikipedia.org/wiki/Taxicab_geometry
        var distanceToBeacon = Math.Abs(sensor.X - beacon.X) + Math.Abs(sensor.Y - beacon.Y);

        var upperCorner = new Coordinate(sensor.X, sensor.Y - distanceToBeacon);
        var bottomCorner = new Coordinate(sensor.X, sensor.Y + distanceToBeacon);
        if (upperCorner.Y > targetY || bottomCorner.Y < targetY)
        {
            continue;
        }


        var distanceToTarget = targetY > sensor.Y
            ? (sensor.Y + distanceToBeacon) - targetY
            : targetY - (sensor.Y - distanceToBeacon);

        for (int i = sensor.X - distanceToTarget; i < sensor.X + distanceToTarget; i++)
        {
            var c = new Coordinate(i, targetY);
            result.Add(c);
        }
    }

    return result.Count();
}

int Parse(Match match)
{
    return int.Parse(match.Groups[1].Value);
}

[DebuggerDisplay("X:{X} Y:{Y}")]
public record struct Coordinate(int X, int Y)
{

    public IEnumerable<Coordinate> DiagonalPathTo(Coordinate other)
    {
        yield return this;

        Coordinate previous = this;
        while (previous != other)
        {
            var x = 0;
            if (previous.X < other.X)
            {
                x = previous.X + 1;
            }
            else
            {
                x = previous.X - 1;
            }

            var y = 0;
            if (previous.Y < other.Y)
            {
                y = previous.Y + 1;
            }
            else
            {
                y = previous.Y - 1;
            }

            var next = new Coordinate(x, y);
            previous = next;
            yield return next;
        }
    }
}