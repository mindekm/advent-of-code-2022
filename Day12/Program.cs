var lines = File.ReadAllLines(@"input.txt");

var map = new Dictionary<Coordinate, Symbol>();
for (var i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[0].Length; j++)
    {
        map[new Coordinate(j, i)] = new Symbol(lines[i][j]);
    }
}

var result1 = Search(map).Single(l => l.Symbol == new Symbol('S'));
Console.WriteLine(result1.DistanceFromDestination);

var result2 = Search(map)
    .Where(l => l.Symbol.GetElevation() == 'a')
    .Select(l => l.DistanceFromDestination)
    .Min();
Console.WriteLine(result2);

// https://en.wikipedia.org/wiki/Breadth-first_search
IEnumerable<Location> Search(Dictionary<Coordinate, Symbol> map)
{
    var destination = map.Keys.Single(c => map[c] == new Symbol('E'));
    var locations = new Dictionary<Coordinate, Location>
    {
        [destination] = new Location{Symbol = new Symbol('E'), DistanceFromDestination = 0}
    };

    var queue = new Queue<Coordinate>();
    queue.Enqueue(destination);
    while (queue.TryDequeue(out var coordinate))
    {
        var location = locations[coordinate];
        foreach (var nextCoordinate in coordinate.Neighbours().Where(map.ContainsKey))
        {
            if (locations.ContainsKey(nextCoordinate))
            {
                continue;
            }

            var nextSymbol = map[nextCoordinate];
            if (location.Symbol.GetElevation() - nextSymbol.GetElevation() <= 1)
            {
                locations[nextCoordinate] = new Location
                {
                    Symbol = nextSymbol,
                    DistanceFromDestination = location.DistanceFromDestination + 1,
                };
                queue.Enqueue(nextCoordinate);
            }
        }
    }

    return locations.Values;
}

public sealed class Location
{
    public required Symbol Symbol { get; init; }

    public required int DistanceFromDestination { get; init; }
}

public record Symbol(char Value)
{
    public char GetElevation()
    {
        return Value switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => Value,
        };
    }
}

public record Coordinate(int X, int Y)
{
    public IEnumerable<Coordinate> Neighbours()
    {
        yield return this with {X = X + 1};
        yield return this with {X = X - 1};
        yield return this with {Y = Y + 1};
        yield return this with {Y = Y - 1};
    }
}