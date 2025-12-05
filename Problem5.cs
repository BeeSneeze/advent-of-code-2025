using Godot;
using System;
using System.Collections.Generic;

public partial class Problem5 : Node2D
{

    private List<(long,long)> rangeList = new List<(long,long)>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var ranges = ParseData(LoadFromFile("res://problem_5_ranges.txt"));

        

        foreach(var item in ranges)
        {
            GD.Print(item);
            var nums = item.Split('-');
            rangeList.Add((long.Parse(nums[0]), long.Parse(nums[1])));
        }

        var ingredients = ParseData(LoadFromFile("res://problem_5_ingredients.txt"));

        List<long> ingredientList = new List<long>();

        foreach(var item in ingredients)
        {
            ingredientList.Add(long.Parse(item));
        }

        var totalFresh = 0;

        foreach(var ingredient in ingredientList)
        {
            if(CheckExpired(ingredient))
            {
                totalFresh++;
            }
        }

        GD.Print(totalFresh);

    }

    private bool CheckExpired(long ingredient)
    {
        foreach((long,long) r in rangeList)
        {
            if(r.Item1 <= ingredient && r.Item2 >= ingredient)
            {
                return true;
            }
        }


        return false;
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
