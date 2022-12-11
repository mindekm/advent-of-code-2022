var lines = File.ReadAllLines("input1.txt");

var result = new Dictionary<int, int>();

var elfId = 0;
foreach(var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        elfId++;
        continue;
    }

    result[elfId] = result.TryGetValue(elfId, out var value)
        ? value + int.Parse(line)
        : int.Parse(line);
}

Console.WriteLine(result.Values.Max());

Console.WriteLine(result.Values.OrderDescending().Take(3).Sum());