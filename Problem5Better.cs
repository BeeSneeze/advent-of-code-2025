using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public partial class Problem5Better : Node2D
{
    private List<(long,long)> rangeList = new List<(long,long)>();

    public override void _Ready()
    {
        var ranges = ParseData(LoadFromFile("res://problem_5_ranges.txt"));
        foreach(var item in ranges)
        {
            var nums = item.Split('-');
            rangeList.Add((long.Parse(nums[0]), long.Parse(nums[1])));
        }

        // Smush together all range edges into one list, and sort it
        List<(long number,bool isRightEnd)> allSmushed = new List<(long,bool)>();
        foreach(var item in rangeList)
        {
            allSmushed.Add((item.Item1, false));
            allSmushed.Add((item.Item2, true));
        }
        allSmushed = allSmushed.OrderBy(x => x.number).ToList();
        
        // Depending on if it's a right or left edge, create new regions
        long leftEdge = 0;
        var bracketCount = 0;
        List<(long leftEdge, long rightEdge)> fixedRanges = new List<(long, long)>();
        foreach(var tuple in allSmushed)
        {
            if(tuple.isRightEnd)
            {
                // Right bracket
                bracketCount--;
                if(bracketCount == 0)
                    fixedRanges.Add((leftEdge, tuple.number));
            }
            else
            {
                // Left bracket
                if(bracketCount == 0)
                    leftEdge = tuple.number;
                bracketCount++;
            }
        }

        // Use the sorted regions to count valid ranges
        long totalIDs = 0;
        long lastRight = -1;
        foreach(var freshRange in fixedRanges)
        {
            // Avoid double counting via overlapping intervals
            if(lastRight == freshRange.leftEdge)
                totalIDs--;
            totalIDs += freshRange.rightEdge - freshRange.leftEdge + 1;
            lastRight = freshRange.rightEdge;
        }

        GD.Print(totalIDs);
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
