using System.Globalization;

var lines = File.ReadAllLines(@"input.txt");

var grid = new int[lines.Length, lines[0].Length];
for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    for (var j = 0; j < line.Length; j++)
    {
        var character = line[j];
        grid[i, j] = CharUnicodeInfo.GetDecimalDigitValue(character);
    }
}

var result1 = 0;
for (var i = 0; i < grid.GetLength(0); i++)
{
    for (var j = 0; j < grid.GetLength(1); j++)
    {
        var height = grid[i, j];

        if (LeftDirection(i, j).All(other => height > other))
        {
            result1++;
            continue;
        }

        if (UpDirection(i, j).All(other => height > other))
        {
            result1++;
            continue;
        }

        if (RightDirection(i, j).All(other => height > other))
        {
            result1++;
            continue;
        }

        if (DownDirection(i, j).All(other => height > other))
        {
            result1++;
            continue;
        }
    }
}

Console.WriteLine(result1);

var result2 = 0;
for (var i = 0; i < grid.GetLength(0); i++)
{
    for (var j = 0; j < grid.GetLength(1); j++)
    {
        var height = grid[i, j];
        
        var left = ViewingDistance(LeftDirection(i, j), height);
        var up = ViewingDistance(UpDirection(i, j), height);
        var right = ViewingDistance(RightDirection(i, j), height);
        var down = ViewingDistance(DownDirection(i, j), height);

        var score = left * up * right * down;
        if (score > result2)
        {
            result2 = score;
        }
    }
}

Console.WriteLine(result2);

int ViewingDistance(IEnumerable<int> trees, int height)
{
    var result = 0;

    foreach (var tree in trees)
    {
        if (tree >= height)
        {
            result++;
            break;
        }

        result++;
    }

    return result;
}

IEnumerable<int> UpDirection(int verticalCoordinate, int horizontalCoordinate)
{
    verticalCoordinate--;
    
    while (verticalCoordinate >= 0)
    {
        var height = grid[verticalCoordinate, horizontalCoordinate];
        yield return height;
        
        verticalCoordinate--;
    }
}

IEnumerable<int> DownDirection(int verticalCoordinate, int horizontalCoordinate)
{
    verticalCoordinate++;
    
    while (verticalCoordinate <= grid.GetLength(0) - 1)
    {
        var height = grid[verticalCoordinate, horizontalCoordinate];
        yield return height;
        
        verticalCoordinate++;
    }
}

IEnumerable<int> LeftDirection(int verticalCoordinate, int horizontalCoordinate)
{
    horizontalCoordinate--;
    
    while (horizontalCoordinate >= 0)
    {
        var height = grid[verticalCoordinate, horizontalCoordinate];
        yield return height;

        horizontalCoordinate--;
    }
}

IEnumerable<int> RightDirection(int verticalCoordinate, int horizontalCoordinate)
{
    horizontalCoordinate++;
    
    while (horizontalCoordinate <= grid.GetLength(1) -1)
    {
        var height = grid[verticalCoordinate, horizontalCoordinate];
        yield return height;

        horizontalCoordinate++;
    }
}
