var lines = File.ReadAllLines(@"input.txt");

var xRegister = 1;
var registerValues = new List<int>(0) {0, 1};
foreach (var line in lines)
{
    var split = line.Split(" ");

    if (split.Length == 1)
    {
        registerValues.Add(xRegister);
    }
    else
    {
        var value = int.Parse(split[1]);

        registerValues.Add(xRegister);
        xRegister += value;
        registerValues.Add(xRegister);
    }
}

var result = 0;
foreach (var cycle in new[] {20, 60, 100, 140, 180, 220})
{
    result += registerValues[cycle] * cycle;
}

Console.WriteLine(result);

foreach (var row in registerValues.Skip(1).Chunk(40))
{
    Console.WriteLine();
    for (var i = 0; i < row.Length; i++)
    {
        var value = row[i];
        if (i >= value - 1 && i <= value + 1)
        {
            Console.Write("#");
        }
        else
        {
            Console.Write(".");
        }
    }
}
