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

        var numOfRanges = rangeList.Count;


        var foundViableMerge = true;

        while(foundViableMerge)
        {
            (int,int) mergeIndices = new (-1,-1);
            foundViableMerge = false;
            for(int i = 0; i < numOfRanges; i++)
            {
                for(int j = 0; j < numOfRanges; j++)
                {
                    if(i==j)
                    {
                        continue;
                    }

                    if(ShouldMerge(rangeList[i].Item1, rangeList[i].Item2, rangeList[j].Item1, rangeList[j].Item2))
                    {
                        foundViableMerge = true;
                        mergeIndices = (i,j);
                        break;
                    }
                }
                if(foundViableMerge)
                {
                    break;
                }
            }

            if(foundViableMerge)
            {
                // PERFORM MERGE
                GD.Print("DID A MERGE!");
                numOfRanges--;
                long leftNew = 0;
                long rightNew = 0;
                if(rangeList[mergeIndices.Item1].Item1 < rangeList[mergeIndices.Item2].Item1)
                {
                    leftNew = rangeList[mergeIndices.Item1].Item1;
                }
                else
                {
                    leftNew = rangeList[mergeIndices.Item2].Item1;
                }

                if(rangeList[mergeIndices.Item1].Item2 > rangeList[mergeIndices.Item2].Item2)
                {
                    rightNew = rangeList[mergeIndices.Item1].Item2;
                }
                else
                {
                    rightNew = rangeList[mergeIndices.Item2].Item2;
                }

                GD.Print(rangeList[mergeIndices.Item1]);
                GD.Print(rangeList[mergeIndices.Item2]);

                rangeList.Remove(rangeList[mergeIndices.Item1]);
                rangeList.Remove(rangeList[mergeIndices.Item2-1]);
                rangeList.Add((leftNew,rightNew));
                GD.Print((leftNew,rightNew));                
            }
            
        }

        GD.Print(rangeList.Count);

        long totalIDs = 0;

        foreach(var freshRange in rangeList)
        {
            totalIDs += (freshRange.Item2 - freshRange.Item1) + 1;
        }

        GD.Print(totalIDs);


    }

    private bool ShouldMerge(long leftA, long rightA, long leftB, long rightB)
    {
        if(leftB <= leftA && rightA <= rightB)
        {
            return true;
        }
        if(leftA <= leftB && rightB <= rightA)
        {
            return true;
        }

        if(leftA <= leftB && leftB <= rightA && rightA <= rightB)
        {
            return true;
        }

        if(leftB <= leftA && leftA <= rightB && rightB <= rightA)
        {
            return true;
        }

        return false;

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
