var lines = File.ReadAllLines(@"input1.txt");

var overlap = 0;
var partialOverlap = 0;
foreach (var line in lines)
{
    var assignments = line.Split(",").Select(e => e.Split("-")).ToList();

    var start1 = int.Parse(assignments[0][0]);
    var end1 = int.Parse(assignments[0][1]);

    var start2 = int.Parse(assignments[1][0]);
    var end2 = int.Parse(assignments[1][1]);

    if ((start2 >= start1 && end2 <= end1) || (start1 >= start2 && end1 <= end2))
    {
        overlap++;
    }

    if ((end1 < start2) || (start1 > end2))
    {
        continue;
    }
    else
    {
        partialOverlap++;
    }
}

Console.WriteLine(overlap);
Console.WriteLine(partialOverlap);