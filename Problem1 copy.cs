using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Problem1Copy : Node2D
{
    string unparsedData = "";
    string [] parsedData = [];

    int dialPosition = 50;
    int totalZeroes = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        unparsedData = LoadFromFile();
        ParseData(unparsedData);
        foreach(string item in parsedData)
        {
            if(item[0] == 'R')
            {
                dialPosition += item.Substring(1).ToInt();
            }
            else
            {
                dialPosition -= item.Substring(1).ToInt();
            }
            dialPosition %= 100;
            GD.Print(dialPosition);

            if(dialPosition == 0)
            {
                totalZeroes += 1;
            }
        }

        GD.Print(totalZeroes);
        
    }

    private string[] ParseData(string unparsed)
    {
        parsedData = unparsed.Split('\n');
        GD.Print(parsedData.Count());
        return parsedData;
    }

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_1.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }
}
