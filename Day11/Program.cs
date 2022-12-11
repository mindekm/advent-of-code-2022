using System.Diagnostics;

var lines = File.ReadAllLines(@"input.txt");

var monkeys = ParseMonkeys(lines).ToList();
for (var i = 0; i < 20; i++)
{
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.TryDequeue(out var item))
        {
            item = monkey.Inspect(item);
            var destination = monkey.Throw(item);
            monkeys[destination].Receive(item);
        }
    }
}

var result = monkeys.Select(m => m.InspectionCount).OrderDescending().Take(2).ToList();
Console.WriteLine(result[0] * result[1]);


monkeys = ParseMonkeys(lines).ToList();
// https://en.wikipedia.org/wiki/Modular_arithmetic ???
var mod = monkeys.Aggregate(1, (a, m) => a * m.DivisibilityTest);
for (var i = 0; i < 10_000; i++)
{
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.TryDequeue(out var item))
        {
            item = monkey.Inspect(item, mod);
            var destination = monkey.Throw(item);
            monkeys[destination].Receive(item);
        }
    }
}

result = monkeys.Select(m => m.InspectionCount).OrderDescending().Take(2).ToList();
Console.WriteLine(result[0] * result[1]);

IEnumerable<Monkey> ParseMonkeys(IEnumerable<string> lines)
{
    return lines
        .Where(l => !string.IsNullOrWhiteSpace(l))
        .Chunk(6)
        .Select(Monkey.Parse);
}

[DebuggerDisplay("Inspections = {InspectionCount}")]
public sealed class Monkey
{
    public required Queue<long> Items { get; init; }
    
    public required Func<long, long> Operation { get; init; }

    public required int DivisibilityTest { get; init; }

    public required int DestinationIfTrue { get; init; }

    public required int DestinationIfFalse { get; init; }

    public long InspectionCount { get; private set; }

    public static Monkey Parse(string[] definition)
    {
        var monkey = new Monkey
        {
            Items = new Queue<long>(),
            Operation = ParseOperation(definition[2]),
            DivisibilityTest = int.Parse(definition[3].Split(" ").Last()),
            DestinationIfTrue = int.Parse(definition[4].Split(" ").Last()),
            DestinationIfFalse = int.Parse(definition[5].Split(" ").Last()),
        };

        var items = ParseStartingItems(definition[1]);
        foreach (var item in items)
        {
            monkey.Items.Enqueue(item);
        }

        return monkey;
    }

    public long Inspect(long item, int? worryReduction = default)
    {
        InspectionCount++;

        item = Operation(item);
        if (worryReduction.HasValue)
        {
            return item % worryReduction.Value;
        }
        else
        {
            return item / 3;
        }
    }

    public int Throw(long item)
    {
        return item % DivisibilityTest == 0 ? DestinationIfTrue : DestinationIfFalse;
    }

    public void Receive(long item)
    {
        Items.Enqueue(item);
    }
    
    private static IEnumerable<int> ParseStartingItems(string value)
    {
        return value.Split(": ")[1].Split(",").Select(int.Parse);
    }

    private static Func<long, long> ParseOperation(string value)
    {
        var operation = value.Split(" = ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var left = operation[0];
        var @operator = operation[1];
        var right = operation[2];

        return i =>
        {
            if (left == "old" && right == "old")
            {
                return @operator switch
                {
                    "+" => i + i,
                    "*" => i * i,
                    _ => throw new UnreachableException(),
                };
            }
            else
            {
                var constant = int.Parse(right);

                return @operator switch
                {
                    "+" => i + constant,
                    "*" => i * constant,
                    _ => throw new UnreachableException(),
                };
            }
        };
    }
}