var lines = File.ReadAllLines(@"input1.txt");

int PriorityFor(char c) => char.IsUpper(c) ? c - 38 : c - 96;

var totalPriority = 0;
foreach (var line in lines)
{
    if (line.Length % 2 != 0)
    {
        throw new InvalidOperationException();
    }

    var firstCompartment = line[..(line.Length / 2)];
    var secondCompartment = line[(line.Length / 2)..];

    var inBoth = firstCompartment.Intersect(secondCompartment).ToList();
    foreach (var value in inBoth)
    {
        totalPriority += PriorityFor(value);
    }
}

Console.WriteLine(totalPriority);

var totalPriority2 = 0;
foreach (var group in lines.Chunk(3))
{
    var badge = group[0].Intersect(group[1]).Intersect(group[2]).ToList();
    if (badge.Count != 1)
    {
        throw new InvalidOperationException();
    }

    totalPriority2 += PriorityFor(badge[0]);
}

Console.WriteLine(totalPriority2);
