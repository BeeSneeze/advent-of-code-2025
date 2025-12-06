using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public partial class Problem5Better2 : Node2D
{
    private List<(long left, long right)> rangeList = new List<(long,long)>();

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
        Dictionary<long, int> stopGaps = new Dictionary<long, int>();

        foreach(var item in rangeList)
        {
            stopGaps[item.left] += 1;
            stopGaps[item.right] -= 1;
            allSmushed.Add((item.left, false));
            allSmushed.Add((item.right, true));
        }
        allSmushed = allSmushed.OrderBy(x => x.isRightEnd).ToList();
        allSmushed = allSmushed.OrderBy(x => x.number).ToList();
        
        // Depending on if it's a right or left edge, create new regions
        long totalIDs = 0;
        long leftEdge = 0;
        var bracketCount = 0;
        foreach(var tuple in allSmushed)
        {
            if(tuple.isRightEnd)
            {
                // Right bracket
                bracketCount--;
                if(bracketCount == 0)
                    totalIDs += tuple.number - leftEdge + 1;
            }
            else
            {
                // Left bracket
                if(bracketCount == 0)
                    leftEdge = tuple.number;
                bracketCount++;
            }
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
