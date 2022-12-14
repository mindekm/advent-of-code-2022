using System.Diagnostics;

var map = CreateMap();
var result1 = Simulate(map, null);
Console.WriteLine(result1);

map = CreateMap();
var floor = map.Keys.Select(c => c.Y).Max() + 2;
var result2 = Simulate(map, floor);
Console.WriteLine(result2);

int Simulate(Dictionary<Coordinate, bool> map, int? floor)
{
    var result = 0;
    var source = new Coordinate(500, 0);
    while (true)
    {
        var initial = source;
        var falling = true;
        var moves = 0;

        while (falling)
        {
            if (moves >= 200)
            {
                break;
            }

            var moved = false;
            foreach (var candidate in initial.NextCandidates())
            {
                if (candidate.Y == floor)
                {
                    continue;
                }                

                if (!map.GetOrAddEmpty(candidate))
                {
                    map[initial] = false;
                    map[candidate] = true;
                    initial = candidate;
                    moved = true;
                    moves++;

                    break;
                }
            }

            if (!moved)
            {
                falling = false;
                result++;
            }
        }

        // filled
        if (moves == 0)
        {
            break;
        }

        // falling forever
        if (moves >= 200)
        {
            break;
        }
    }

    return result;
}

Dictionary<Coordinate, bool> CreateMap()
{
    var lines = File.ReadAllLines(@"input.txt");

    var map = new Dictionary<Coordinate, bool>();
    foreach (var line in lines)
    {
        var split = line.Split(" -> ");
        for (var i = 0; i < split.Length; i++)
        {
            if (i == 0)
            {
                map[Coordinate.Parse(split[i])] = true;

                continue;
            }

            var start = Coordinate.Parse(split[i - 1]);
            var end = Coordinate.Parse(split[i]);

            foreach (var coordinate in start.PathTowards(end))
            {
                map[coordinate] = true;
            }
        }
    }

    return map;
}

public static class Ext
{
    public static bool GetOrAddEmpty(this Dictionary<Coordinate, bool> dict, Coordinate coord)
    {
        if(dict.TryGetValue(coord, out var result))
        {
            return result;
        }
        else
        {
            dict[coord] = false;
            return false;
        }
    }
}

[DebuggerDisplay("X:{X} Y:{Y}")]
public record Coordinate(int X, int Y)
{
    public static Coordinate Parse(string value)
    {
        var split = value.Split(',');

        return new Coordinate(int.Parse(split[0]), int.Parse(split[1]));
    }

    public Coordinate Left() => new Coordinate(X - 1, Y);
    public Coordinate Right() => new Coordinate(X + 1, Y);
    public Coordinate Up() => new Coordinate(X, Y - 1);
    public Coordinate Down() => new Coordinate(X, Y + 1);

    public IEnumerable<Coordinate> NextCandidates()
    {
        yield return new Coordinate(X, Y + 1);
        yield return new Coordinate(X - 1, Y + 1);
        yield return new Coordinate(X + 1, Y + 1);
    }

    public IEnumerable<Coordinate> PathTowards(Coordinate destination)
    {
        if (this == destination)
        {
            yield break;
        }

        if (destination.X == X)
        {
            if (destination.Y > Y)
            {
                var next = Down();
                while (next != destination)
                {
                    yield return next;
                    next = next.Down();
                }
            }
            else
            {
                var next = Up();
                while (next != destination)
                {
                    yield return next;
                    next = next.Up();
                }
            }
        }

        if (destination.Y == Y)
        {
            if (destination.X > X)
            {
                var next = Right();
                while (next != destination)
                {
                    yield return next;
                    next = next.Right();
                }
            }
            else
            {
                var next = Left();
                while(next != destination)
                {
                    yield return next;
                    next = next.Left();
                }
            }

        }

        yield return destination;
    }
}