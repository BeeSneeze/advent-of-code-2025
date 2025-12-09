using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class Problem9Copy : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var data = ParseData(LoadFromFile("res://problem_9.txt"));
        //var redList = InitExample();

        var redList = new List<(long x, long y)>();

        foreach(var row in data)
        {
            redList.Add((row.Split(',')[0].ToInt(), row.Split(',')[1].ToInt()));
        }

        var biggestRectangle = 0L;

        for(int a = 0; a < redList.Count; a++)
        {
            for(int b = 0; b < redList.Count; b++)
            {
                var rectSize =(1 + Math.Abs(redList[a].x - redList[b].x)) * (1 + Math.Abs(redList[a].y - redList[b].y));
                if(rectSize > biggestRectangle)
                {
                    biggestRectangle = rectSize;
                }
            }
        }

        GD.Print(biggestRectangle);

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
}
