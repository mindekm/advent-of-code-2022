using System.Text.Json.Nodes;

var lines1 = File.ReadAllLines(@"input.txt");
var packets1 = lines1
    .Where(l => !string.IsNullOrWhiteSpace(l))
    .Select(l => JsonNode.Parse(l))
    .Chunk(2);

var result1 = 0;
foreach (var (pair, index) in packets1.Select((p, i) => (p, i)))
{
    var left = pair[0];
    var right = pair[1];

    if (NodeComparer.Instance.Compare(left, right) < 0)
    {
        result1 += index + 1;
    }
}

Console.WriteLine(result1);


var lines2 = File.ReadAllLines(@"input.txt");
var packets2 = lines2
    .Where(l => !string.IsNullOrWhiteSpace(l))
    .Select(l => JsonNode.Parse(l))
    .ToList();
var divider1 = JsonNode.Parse("[[2]]");
var divider2 = JsonNode.Parse("[[6]]");
packets2.Add(divider1);
packets2.Add(divider2);

packets2.Sort(NodeComparer.Instance);
var result2 = (packets2.IndexOf(divider1) + 1) * (packets2.IndexOf(divider2) + 1);
Console.WriteLine(result2);


public sealed class NodeComparer : IComparer<JsonNode>
{
    public static readonly IComparer<JsonNode> Instance = new NodeComparer();

    public int Compare(JsonNode? x, JsonNode? y)
    {
        if (x is JsonValue leftValue && y is JsonValue rightValue)
        {
            return leftValue.GetValue<int>() - rightValue.GetValue<int>();
        }

        var leftArray = x as JsonArray ?? new JsonArray(x.GetValue<int>());
        var rightArray = y as JsonArray ?? new JsonArray(y.GetValue<int>());
    
        // Zip
        // The method merges each element of the first sequence with an element that has the same index in the second sequence. If the sequences do not have the same number of elements, the method merges sequences until it reaches the end of one of them
        return leftArray
            .Zip(rightArray)
            .Select(p => Compare(p.First, p.Second))
            .FirstOrDefault(i => i!=0, leftArray.Count.CompareTo(rightArray.Count));
    }
}
