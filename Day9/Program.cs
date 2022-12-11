using System.Diagnostics;

var lines = File.ReadAllLines(@"input.txt");

var rope = new Rope();
var result = new HashSet<(int, int)>();
foreach (var line in lines)
{
    var split = line.Split(" ");
    var direction = split[0];
    var steps = int.Parse(split[1]);

    while (steps != 0)
    {
        switch (direction)
        {
            case "U":
                result.Add(rope.MoveUp());
                break;
            case "R":
                result.Add(rope.MoveRight());
                break;
            case "D":
                result.Add(rope.MoveDown());
                break;
            case "L":
                result.Add(rope.MoveLeft());
                break;
            default:
                throw new UnreachableException();
        }
        
        steps--;
    }
}

Console.WriteLine(result.Count);


var rope2 = new Rope2(10);
var result2 = new HashSet<(int, int)>();
foreach (var line in lines)
{
    var split = line.Split(" ");
    var direction = split[0];
    var steps = int.Parse(split[1]);

    while (steps != 0)
    {
        switch (direction)
        {
            case "U":
                result2.Add(rope2.MoveUp());
                break;
            case "R":
                result2.Add(rope2.MoveRight());
                break;
            case "D":
                result2.Add(rope2.MoveDown());
                break;
            case "L":
                result2.Add(rope2.MoveLeft());
                break;
            default:
                throw new UnreachableException();
        }
        
        steps--;
    }
}

Console.WriteLine(result2.Count);

public sealed class Rope
{
    public int HeadX { get; private set; }

    public int HeadY { get; private set; }

    public int TailX { get; private set; }

    public int TailY { get; private set; }

    public bool IsTailOverlapping => HeadX == TailX && HeadY == TailY;

    public bool IsTailAdjacent
        => (TailX >= HeadX - 1 && TailX <= HeadX + 1) && (TailY >= HeadY - 1 && TailY <= HeadY + 1);

    public bool IsTailTouching => IsTailOverlapping || IsTailAdjacent;

    public (int, int) MoveUp()
    {
        HeadY++;
        MoveTail();

        return (TailX, TailY);
    }

    public (int, int) MoveRight()
    {
        HeadX++;
        MoveTail();

        return (TailX, TailY);
    }

    public (int, int) MoveDown()
    {
        HeadY--;
        MoveTail();

        return (TailX, TailY);
    }

    public (int, int) MoveLeft()
    {
        HeadX--;
        MoveTail();

        return (TailX, TailY);
    }

    private void MoveTail()
    {
        if (IsTailTouching)
        {
            return;
        }

        // move horizontally
        if (HeadX != TailX)
        {
            if (HeadX > TailX)
            {
                TailX++;
            }
            else
            {
                TailX--;
            }
        }

        // move vertically
        if (HeadY != TailY)
        {
            if (HeadY > TailY)
            {
                TailY++;
            }
            else
            {
                TailY--;
            }
        }
    }
}

public sealed class Rope2
{
    public Rope2(int numberOfKnots)
    {
        Segments = new Segment[numberOfKnots];
        for (var i = 0; i < numberOfKnots; i++)
        {
            var identifier = i == 0 ? "H" : i.ToString();
            Segments[i] = new Segment {Identifier = identifier, X = 0, Y = 0};
        }
    }
    
    public Segment[] Segments { get; }

    public (int, int) MoveUp()
    {
        var head = Segments[0];
        head.Y++;
        MoveSegments();

        var tail = Segments.Last();
        return (tail.X, tail.Y);
    }

    public (int, int) MoveRight()
    {
        var head = Segments[0];
        head.X++;
        MoveSegments();

        var tail = Segments.Last();
        return (tail.X, tail.Y);
    }

    public (int, int) MoveDown()
    {
        var head = Segments[0];
        head.Y--;
        MoveSegments();

        var tail = Segments.Last();
        return (tail.X, tail.Y);
    }

    public (int, int) MoveLeft()
    {
        var head = Segments[0];
        head.X--;
        MoveSegments();

        var tail = Segments.Last();
        return (tail.X, tail.Y);
    }

    private void MoveSegments()
    {
        for (var i = 1; i < Segments.Length; i++)
        {
            var previousSegment = Segments[i - 1];
            var segment = Segments[i];

            segment.Follow(previousSegment);
        }
    }
}

[DebuggerDisplay("[{Identifier}] - X={X}, Y={Y}")]
public sealed class Segment
{
    public required string Identifier { get; init; }
    
    public int X { get; set; }

    public int Y { get; set; }

    public bool IsOverlapping(Segment other) => X == other.X && Y == other.Y;

    public bool IsAdjacent(Segment other) =>
        (other.X >= X - 1 && other.X <= X + 1) && (other.Y >= Y - 1 && other.Y <= Y + 1);

    public bool IsTouching(Segment other) => IsOverlapping(other) || IsAdjacent(other);

    public void Follow(Segment previousSegment)
    {
        if (IsTouching(previousSegment))
        {
            return;
        }

        if (previousSegment.X != X)
        {
            if (previousSegment.X > X)
            {
                X++;
            }
            else
            {
                X--;
            }
        }

        if (previousSegment.Y != Y)
        {
            if (previousSegment.Y > Y)
            {
                Y++;
            }
            else
            {
                Y--;
            }
        }
    }
}
