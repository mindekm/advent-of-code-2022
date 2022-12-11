using System.Diagnostics;

var lines = File.ReadAllLines(@"input1.txt");

foreach (var line in lines)
{
    var result = 0;
    // result = StartOfPacket(line);
    result = StartOfMessage(line);
    
    Console.WriteLine(result);
}

int StartOfPacket(string datastream)
{
    var span = datastream.AsSpan();
    var start = 0;

    var set = new HashSet<char>();
    while (start + 4 <= span.Length)
    {
        var slice = span.Slice(start, 4);

        set.Clear();
        foreach (var c in slice)
        {
            set.Add(c);
        }

        if (set.Count == 4)
        {
            return start + 4;
        }

        start++;
    }

    throw new UnreachableException();
}

int StartOfMessage(string datastream)
{
    var span = datastream.AsSpan();
    var start = 0;

    var set = new HashSet<char>();
    while (start + 14 <= span.Length)
    {
        var slice = span.Slice(start, 14);

        set.Clear();
        foreach (var c in slice)
        {
            set.Add(c);
        }

        if (set.Count == 14)
        {
            return start + 14;
        }

        start++;
    }

    throw new UnreachableException();
}