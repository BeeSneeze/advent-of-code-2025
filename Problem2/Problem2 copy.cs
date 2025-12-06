using Godot;
using System;

public partial class Problem2Copy : Node2D
{

    private string[] parsedData;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        long totalInvalidIDs = 0;
        
        var unparsedData = LoadFromFile();
        parsedData = ParseData(unparsedData);

        foreach(var item in parsedData)
        {
            var startNum = long.Parse(item.Split('-')[0]);
            var endNum = long.Parse(item.Split('-')[1]);

            for(long examinedID = startNum; examinedID <= endNum; examinedID++)
            {
                var stringRepresentation = examinedID.ToString();

                
                if(stringRepresentation.Length % 2 == 0)
                {
                    var multiplier = (int)Math.Pow(10, stringRepresentation.Length / 2) + 1;
                    if(examinedID % multiplier == 0)
                    {
                        //GD.Print(multiplier);
                        totalInvalidIDs += examinedID;
                        GD.Print(examinedID);
                    }
                }
            }
        }

        GD.Print(totalInvalidIDs);

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
