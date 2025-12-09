using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Problem9 : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var redList = InitProblem("res://problem_9.txt");

        var redDotList = new List<RedDot>();

        var turnAmount = 0;

        redDotList.Add(new RedDot(redList[0].x, redList[0].y, FindOrientation(redList[redList.Count-1], redList[0]), FindOrientation(redList[0], redList[1])));
        for(int k = 1; k < redList.Count-1; k++)
        {
            var nextDot = new RedDot(redList[k].x, redList[k].y, FindOrientation(redList[k-1], redList[k]), FindOrientation(redList[k], redList[k+1]));
            if(nextDot.ClockWise())
            {
                turnAmount += 1;
            }
            else
            {
                turnAmount -= 1;
            }
            redDotList.Add(nextDot);
        }
        redDotList.Add(new RedDot(redList[redList.Count-1].x, redList[redList.Count-1].y, FindOrientation(redList[redList.Count-2], redList[redList.Count-1]), FindOrientation(redList[redList.Count-1], redList[0])));


        foreach(var dot in redDotList)
        {
            GD.Print(dot);
        }

        // If we do more clockwise turns, that means clockwise turns are inside oriented
        var systemIsClockwise = turnAmount > 0;

        // Step 2: Figure out which ones are inside the area

        var biggestRectangle = 0L;
        
        for(int a = 0; a < redDotList.Count; a++)
        {
            for(int b = 0; b < redDotList.Count; b++)
            {
                var rectSize = (1 + Math.Abs(redDotList[a].X - redDotList[b].X)) * (1 + Math.Abs(redDotList[a].Y - redDotList[b].Y));
                var aDotValid = ValidRectangle(redDotList[a], redDotList[b], systemIsClockwise);
                var bDotValid = ValidRectangle(redDotList[b], redDotList[a], systemIsClockwise);
                var overlapValid = NoOverlaps(redDotList[b], redDotList[a], redDotList, systemIsClockwise);

                if(aDotValid && bDotValid && overlapValid)
                {
                    var validIntersections = NoIntersections(redDotList[b], redDotList[a], redDotList);
                    if(!validIntersections)
                    {
                        //GD.Print("FOUND AN INTERSECTION!!");
                    }
                    if(rectSize > biggestRectangle && validIntersections )
                    {
                        biggestRectangle = rectSize;
                    }
                }

                
            }
        }

        GD.Print(biggestRectangle);
    }

    // Too Low:  93328320
    // Too High: 4790063600
    // Too High: 4583207265
    // Wrong   : 4266799061
    // ???     : 1516172795



    private bool IsBetween(long checkedValue, long sideA, long sideB)
    {
        if(sideA > sideB)
        {
            return checkedValue < sideA && checkedValue > sideB;
        }
        else
        {
            return checkedValue > sideA && checkedValue < sideB;
        }
    }

    private bool NoIntersections(RedDot activeDot, RedDot comparisonDot, List<RedDot> dotList)
    {
        for(int k = 0; k < dotList.Count-1; k++)
        {
            if(dotList[k].X - dotList[k+1].X == 0)
            {
                if(IsBetween(activeDot.Y, dotList[k].Y, dotList[k+1].Y) || IsBetween(comparisonDot.Y, dotList[k].Y, dotList[k+1].Y))
                {
                    if(IsBetween(dotList[k].X, activeDot.X, comparisonDot.X))
                    {
                        //GD.Print(dotList[k], dotList[k+1]);
                        //GD.Print(activeDot, comparisonDot);
                        return false;
                    }
                }
            }
            else
            {
                if(IsBetween(activeDot.X, dotList[k].X, dotList[k+1].X) || IsBetween(comparisonDot.X, dotList[k].X, dotList[k+1].X))
                {
                    if(IsBetween(dotList[k].Y, activeDot.Y, comparisonDot.Y))
                    {
                        //GD.Print(dotList[k], dotList[k+1]);
                        //GD.Print(activeDot, comparisonDot);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private bool NoOverlaps(RedDot activeDot, RedDot comparisonDot, List<RedDot> dotList, bool systemClockwise)
    {
        var above = activeDot.Y > comparisonDot.Y;
        var toTheRight = activeDot.X > comparisonDot.X;

        foreach(var dot in dotList)
        {
            if(dot.ClockWise() != systemClockwise)
            {
                continue;
            }


            if(dot.X == activeDot.X || dot.X == comparisonDot.X)
            {
                if(above && dot.Y > comparisonDot.Y && dot.Y < activeDot.Y)
                {
                    return false;
                }
                else if(!above && dot.Y > activeDot.Y && dot.Y < comparisonDot.Y)
                {
                    return false;
                }
            }
            if(dot.Y == activeDot.Y || dot.Y == comparisonDot.Y)
            {
                if(toTheRight && dot.X > comparisonDot.X && dot.X < activeDot.X)
                {
                    return false;
                }
                else if(!toTheRight && dot.X > activeDot.X && dot.X < comparisonDot.X)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool ValidRectangle(RedDot activeDot, RedDot comparisonDot, bool systemClockwise)
    {
        if(activeDot.ClockWise() == systemClockwise)
        {
            // If a dot is clockwise while the system is clockwise, only two edges are valid
            if(activeDot.X > comparisonDot.X)
            {
                // Active is to the right
                if(activeDot.InDir == Orientation.LEFT)
                {
                    return false;
                }
                if(activeDot.OutDir == Orientation.RIGHT)
                {
                    return false;
                }
            }
            if(activeDot.X < comparisonDot.X)
            {
                // Active is to the left
                if(activeDot.InDir == Orientation.RIGHT)
                {
                    return false;
                }
                if(activeDot.OutDir == Orientation.LEFT)
                {
                    return false;
                }
            }

            if(activeDot.Y > comparisonDot.Y)
            {
                // Active is above
                if(activeDot.InDir == Orientation.DOWN)
                {
                    return false;
                }
                if(activeDot.OutDir == Orientation.UP)
                {
                    return false;
                }
            }
            if(activeDot.Y < comparisonDot.Y)
            {
                // Active is below
                if(activeDot.InDir == Orientation.UP)
                {
                    return false;
                }
                if(activeDot.OutDir == Orientation.DOWN)
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            // If system and corner are different, that's an inward pointing turn, and all are valid
            return true;
        }
        
    }

    enum Orientation
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        IMMOBILE
    }

    private Orientation FindOrientation((long x, long y) a, (long x, long y) b)
    {

        if(a.x == b.x)
        {
            if(a.y == b.y)
            {
                return Orientation.IMMOBILE;
            }
            if(a.y < b.y)
            {
                return Orientation.UP;
            }
            else
            {
                return Orientation.DOWN;
            }
        }
        else
        {
            if(a.x < b.x)
            {
                return Orientation.RIGHT;
            }
            else
            {
                return Orientation.LEFT;
            }
        }
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

    private List<(int x, int y)> InitExample()
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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

    
    record struct RedDot(long X, long Y, Orientation InDir, Orientation OutDir)
    {
        public override string ToString()
        {
            return X.ToString() + ", " + Y.ToString() + ": " + InDir.ToString() + OutDir.ToString() + ", " + ClockWise();
        }

        public bool ClockWise()
        {
            if(InDir == Orientation.UP && OutDir == Orientation.RIGHT)
            {
                return true;
            }
            else if(InDir == Orientation.RIGHT && OutDir == Orientation.DOWN)
            {
                return true;
            }
            else if(InDir == Orientation.DOWN && OutDir == Orientation.LEFT)
            {
                return true;
            }
            else if(InDir == Orientation.LEFT && OutDir == Orientation.UP)
            {
                return true;
            }
            else
            {
                return false; // Counter Clockwise
            }
        }
    };

}
