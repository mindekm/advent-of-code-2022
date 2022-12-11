var moves = File.ReadAllLines(@"input1.txt").Select(l => l.Split(" ")).ToList();

var stacks1 = ReadStacks();
foreach (var move in moves)
{
    var amount = int.Parse(move[1]);
    var sourceIndex = int.Parse(move[3]) - 1;
    var destinationIndex = int.Parse(move[5]) - 1;

    while (amount != 0)
    {
        var crate = stacks1[sourceIndex].Pop();
        stacks1[destinationIndex].Push(crate);

        amount--;
    }
}

var result1 = string.Empty;
foreach (var stack in stacks1)
{
    if (stack.TryPop(out var c))
    {
        result1 += c;
    }
}

Console.WriteLine(result1);


var stacks2 = ReadStacks();
foreach (var move in moves)
{
    var amount = int.Parse(move[1]);
    var sourceIndex = int.Parse(move[3]) - 1;
    var destinationIndex = int.Parse(move[5]) - 1;
    var buffer = new Stack<char>();

    while (amount != 0)
    {
        var crate = stacks2[sourceIndex].Pop();
        buffer.Push(crate);

        amount--;
    }

    while (buffer.TryPop(out var crate))
    {
        stacks2[destinationIndex].Push(crate);
    }
}

var result2 = string.Empty;
foreach (var stack in stacks2)
{
    if (stack.TryPop(out var c))
    {
        result2 += c;
    }
}

Console.WriteLine(result2);

List<Stack<char>> ReadStacks()
{
    var lines = File.ReadAllLines(@"input2.txt").Reverse();

    var result = new List<Stack<char>>();
    foreach (var line in lines)
    {
        var column = 1;

        
        for (var i = 1; i < line.Length; i+=4)
        {
            if (result.Count < column)
            {
                result.Add(new Stack<char>());
            }
            
            var c = line[i];
            if (!char.IsWhiteSpace(c))
            {
                result[column-1].Push(c);
            }

            column++;
        }
    }

    return result;
}