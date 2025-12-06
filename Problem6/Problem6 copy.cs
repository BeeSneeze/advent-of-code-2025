using Godot;
using System;
using System.Linq;
using System.Numerics;

public partial class Problem6Copy : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var worksheetRows = ParseData(LoadFromFile("res://problem_6.txt"));


        var numberRowStrings = worksheetRows[0..4].Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

        var operationString = worksheetRows.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

        int maxIndex = operationString.Length;

        long totalValues = 0;

        for(int i = 0; i < maxIndex; i++)
        {
            switch(operationString[i])
            {
                case "+":
                    totalValues += long.Parse(numberRowStrings[0][i]) + long.Parse(numberRowStrings[1][i]) + long.Parse(numberRowStrings[2][i]) + long.Parse(numberRowStrings[3][i]);
                break;
                case "*":
                    totalValues += long.Parse(numberRowStrings[0][i]) * long.Parse(numberRowStrings[1][i]) * long.Parse(numberRowStrings[2][i]) * long.Parse(numberRowStrings[3][i]);
                break;
            }

        }


        GD.Print(totalValues);


        


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
