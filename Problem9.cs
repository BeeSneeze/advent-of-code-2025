using Godot;
using System;
using System.Collections.Generic;

public partial class Problem9 : Control
{
    public override void _Ready()
    {
        var redList = InitProblem("res://problem_9.txt");
        var dotList = new List<RedDot>();
        var turnAmount = 0;

        // The starting point wraps around to the end
        dotList.Add(new RedDot(redList[0].x, redList[0].y, FindOrientation(redList[redList.Count-1], redList[0]), FindOrientation(redList[0], redList[1])));
        for(int k = 1; k < redList.Count-1; k++)
        {
            var current = redList[k];
            var next = redList[(k+1) % (redList.Count-1)];
            var currentDot = new RedDot(current.x, current.y, FindOrientation(redList[k-1], current), FindOrientation(current, next));

            if(currentDot.ClockWise())
            {
                turnAmount += 1;
            }
            else
            {
                turnAmount -= 1;
            }
            dotList.Add(currentDot);
        }
        
        // If we do more clockwise turns, that means clockwise turns are inside oriented
        var systemIsClockwise = turnAmount > 0;

        var biggestRectangle = 0L;        
        for(int a = 0; a < dotList.Count; a++)
        {
            for(int b = 0; b < a; b++)
            {
                // The two corners of the rectangle
                var dotA = dotList[a];
                var dotB = dotList[b];

                var turnsValid = ValidRectangleCorner(dotA, dotB, systemIsClockwise) && ValidRectangleCorner(dotB, dotA, systemIsClockwise);

                if(turnsValid && NoOverlaps(dotA, dotB, dotList, systemIsClockwise))
                {
                    var rectSize = (1 + Math.Abs(dotA.X - dotB.X)) * (1 + Math.Abs(dotA.Y - dotB.Y));

                    if(rectSize > biggestRectangle && NoIntersections(dotA, dotB, dotList))
                    {
                        biggestRectangle = rectSize;
                    }
                }
            }
        }

        GD.Print(biggestRectangle);
    }

    record struct RedDot(long X, long Y, Orientation InDir, Orientation OutDir)
    {
        public bool ClockWise()
        {
            if(InDir == Orientation.UP && OutDir == Orientation.RIGHT)
                return true;
            else if(InDir == Orientation.RIGHT && OutDir == Orientation.DOWN)
                return true;
            else if(InDir == Orientation.DOWN && OutDir == Orientation.LEFT)
                return true;
            else if(InDir == Orientation.LEFT && OutDir == Orientation.UP)
                return true;

            return false; // Counter Clockwise
        }

        public override string ToString() =>  X.ToString() + ", " + Y.ToString() + ": " + InDir.ToString() + OutDir.ToString();
    };

    enum Orientation
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    
    private Orientation FindOrientation((long x, long y) start, (long x, long y) end)
    {
        if(start.y < end.y && start.x == end.x)
        {
            return Orientation.UP;
        }
        else if(start.y > end.y && start.x == end.x)
        {
            return Orientation.DOWN;
        }
        else if(start.x < end.x)
        {
            return Orientation.RIGHT;
        }
        else
        {
            return Orientation.LEFT;
        }
    }

    // Checks if a value is inbetween two others
    private bool IsBetween(long checkedValue, long sideA, long sideB)
    {
        if(sideA > sideB)
            return checkedValue < sideA && checkedValue > sideB;
        
        return checkedValue > sideA && checkedValue < sideB;
    }

    // Checks that no red dots overlap the rectangle lines. If OUTER TURNS overlap, that means the valid area 
    // is turning away during the rectangle construction which means that the rectangle can't be valid. INNER TURNS are fine.
    private bool NoOverlaps(RedDot activeDot, RedDot comparisonDot, List<RedDot> dotList, bool systemClockwise)
    {
        foreach(var dot in dotList)
        {
            if(dot.ClockWise() != systemClockwise) // Inward turn is always fine, ignore
                continue;
            if((dot.X == activeDot.X || dot.X == comparisonDot.X) && IsBetween(dot.Y, activeDot.Y, comparisonDot.Y))
                return false;
            if((dot.Y == activeDot.Y || dot.Y == comparisonDot.Y) && IsBetween(dot.X, activeDot.X, comparisonDot.X))
                return false;
        }

        return true;
    }

    // Makes sure that no lines for the valid area are intersecting the rectangle.
    // This function assumes that no points are overlapping the rectangle lines!!
    private bool NoIntersections(RedDot activeDot, RedDot comparisonDot, List<RedDot> dotList)
    {
        for(int k = 0; k < dotList.Count; k++)
        {
            var current = dotList[k];
            var next = dotList[(k+1) % (dotList.Count-1)];

            if(current.X - next.X == 0)
            {
                // Check if an UP-DOWN oriented line is being intersected by the rectangle
                if(IsBetween(activeDot.Y, current.Y, next.Y) || IsBetween(comparisonDot.Y, current.Y, next.Y))
                {
                    if(IsBetween(current.X, activeDot.X, comparisonDot.X))
                        return false;
                }
            }
            else
            {
                // Check if a RIGHT-LEFT oriented line is being intersected by the rectangle
                if(IsBetween(activeDot.X, current.X, next.X) || IsBetween(comparisonDot.X, current.X, next.X))
                {
                    if(IsBetween(current.Y, activeDot.Y, comparisonDot.Y))
                        return false;
                }
            }
        }

        return true;
    }
    
    // Checks if a rectangle construction would end up outside the valid area due to turn orientation
    private bool ValidRectangleCorner(RedDot activeDot, RedDot comparisonDot, bool systemClockwise)
    {
        if(activeDot.ClockWise() == systemClockwise) // If a dot is clockwise while the system is clockwise, only two edges are valid
        {
            if(activeDot.X > comparisonDot.X)
            {
                if(activeDot.InDir == Orientation.LEFT || activeDot.OutDir == Orientation.RIGHT)
                    return false;
            }
            if(activeDot.X < comparisonDot.X)
            {
                if(activeDot.InDir == Orientation.RIGHT || activeDot.OutDir == Orientation.LEFT)
                    return false;
            }
            if(activeDot.Y > comparisonDot.Y)
            {
                if(activeDot.InDir == Orientation.DOWN || activeDot.OutDir == Orientation.UP)
                    return false;
            }
            if(activeDot.Y < comparisonDot.Y)
            {
                if(activeDot.InDir == Orientation.UP || activeDot.OutDir == Orientation.DOWN)
                    return false;
            }
        }
        return true; // If system and corner are different, that's an inward pointing turn, and all rectangles are valid
    }

    private List<(long x, long y)> InitProblem(string path)
    {
        var data = ParseData(LoadFromFile(path));
        var redList = new List<(long x, long y)>();
        foreach(var row in data)
        {
            redList.Add((row.Split(',')[0].ToInt(), row.Split(',')[1].ToInt()));
        }
        return redList;
    }

    private List<(int x, int y)> InitFromExampleGraph()
    {
        var data = ParseData(LoadFromFile("res://problem_9_example.txt"));
        List<(int x, int y)> redList = new List<(int, int)>();
        for(int x = 0; x < data.Length; x++)
        {
            for(int y = 0; y < data[0].Length; y++)
            {
                if(data[x][y] == '#')
                {
                    redList.Add((x,y));
                }
            }
        }
        return redList;
    }

    private string[] ParseData(string unparsed)
    {
        var parsedData = unparsed.Split(System.Environment.NewLine);
        return parsedData;
    }

    public string LoadFromFile(string url)
    {
        using var file = FileAccess.Open(url, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }

}
