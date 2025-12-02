using Godot;
using System;
using System.Collections.Generic;

public partial class Problem2 : Node2D
{

    private string[] parsedData;

    private List<long> falseIDs = new List<long>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        long totalInvalidIDs = 0;
        
        var unparsedData = LoadFromFile();

        var mults = FindMultipliers(2);

        foreach(var item in mults)
        {
            GD.Print(item);
        }

        parsedData = ParseData(unparsedData);

        foreach(var item in parsedData)
        {
            var startNum = long.Parse(item.Split('-')[0]);
            var endNum = long.Parse(item.Split('-')[1]);

            for(long examinedID = startNum; examinedID <= endNum; examinedID++)
            {
                var stringRepresentation = examinedID.ToString();

                var multList = FindMultipliers(stringRepresentation.Length);

                foreach(var multiplier in multList)
                {
                    if(examinedID % multiplier == 0)
                    {
                        //GD.Print(multiplier);
                        if(!falseIDs.Contains(examinedID))
                        {
                            falseIDs.Add(examinedID);
                            totalInvalidIDs += examinedID;
                        }
                        else
                        {
                            GD.Print("DUPLICATE FOUND!!");
                        }
                        
                        //GD.Print(examinedID);
                    }
                }
                
            }
        }

        GD.Print(totalInvalidIDs);

    }


    private List<long> FindMultipliers(int length)
    {
        var multlist = new List<long>();
        int[] divisionList = {2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};

        // ,3,4,5,6,7,8,9,10,11,12


        foreach(var division in divisionList)
        {
            if(length % division == 0)
            {
                var fullSegment = "1";
                //var segment = "1";

                for(int i = 1; i < division; i++)
                {
                    //GD.Print("BLORP");
                    //GD.Print(division, i);
                    for(int j = 1; j < length / division; j++)
                    {
                        fullSegment = "0" + fullSegment;
                    }
                    fullSegment = "1" + fullSegment;

                }

                //GD.Print(fullSegment);
                multlist.Add(long.Parse(fullSegment));
            }
        }

        
        return multlist;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_2.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }

    private string[] ParseData(string unparsed)
    {
        parsedData = unparsed.Split(',');
        return parsedData;
    }
}
