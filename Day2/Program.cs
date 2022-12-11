/*
 * A, X - Rock, 1 point
 * B, Y - Paper, 2 point
 * C, Z - Scissors, 3 points
 *
 * Win - 6 points
 * Draw - 3 points
 * Loss - 0 points
*/

var points = new Dictionary<string, int>
{
    ["X"] = 1,
    ["Y"] = 2,
    ["Z"] = 3,
};
var combinations1 = new Dictionary<string, int>()
{
    ["AX"] = 3,
    ["BY"] = 3,
    ["CZ"] = 3,
    ["AY"] = 6,
    ["BZ"] = 6,
    ["CX"] = 6,
    ["AZ"] = 0,
    ["BX"] = 0,
    ["CY"] = 0,
};

var score1 = 0;
foreach (var line in File.ReadAllLines(@"input1.txt"))
{
    var split = line.Split(" ");

    var opponent = split[0];
    var me = split[1];
    
    score1 += combinations1[string.Concat(opponent, me)] + points[me];
}

Console.WriteLine(score1);

var combinations2 = new Dictionary<string, string>()
{
    ["AX"] = "Z",
    ["BX"] = "X",
    ["CX"] = "Y",
    ["AY"] = "X",
    ["BY"] = "Y",
    ["CY"] = "Z",
    ["AZ"] = "Y",
    ["BZ"] = "Z",
    ["CZ"] = "X",
};

var score2 = 0;
foreach (var line in File.ReadAllLines(@"input2.txt"))
{
    var split = line.Split(" ");

    var opponent = split[0];
    var me = combinations2[string.Concat(split[0], split[1])];

    score2 += combinations1[string.Concat(opponent, me)] + points[me];
}

Console.WriteLine(score2);
