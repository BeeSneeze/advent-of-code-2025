using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class Problem6 : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var worksheetRows = ParseData(LoadFromFile("res://problem_6.txt"));


        var numberRowStrings = worksheetRows[0..4];

        var totalChars = numberRowStrings[0].Length;

        List<long[]> longList = new List<long[]>();

        long[] savedNumbers = new long[4];
        int numberCount = 0;
        for(int k = totalChars-1; k >= 0; k--)
        {
            if(numberRowStrings[0][k] == ' ' && numberRowStrings[1][k] == ' ' && numberRowStrings[2][k] == ' ' && numberRowStrings[3][k] == ' ')
            {
                numberCount = 0;
                longList.Add(savedNumbers);
                savedNumbers = new long[4];
            }
            else
            {
                var numberString = numberRowStrings[0][k].ToString() + numberRowStrings[1][k].ToString() + numberRowStrings[2][k].ToString() + numberRowStrings[3][k].ToString();
                savedNumbers[numberCount] = long.Parse(numberString);

                numberCount++;
            }
        }
        longList.Add(savedNumbers);

        var operationString = worksheetRows.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
        operationString = operationString.Reverse().ToArray();
        int maxIndex = operationString.Length;
        long totalValues = 0;
        for(int i = 0; i < maxIndex; i++)
        {
            GD.Print(longList[i][0]);
            GD.Print(longList[i][1]);
            GD.Print(longList[i][2]);
            GD.Print(longList[i][3]);
            switch(operationString[i])
            {
                case "+":
                    totalValues += longList[i][0] + longList[i][1] + longList[i][2] + longList[i][3];
                break;
                case "*":
                    if(longList[i][0] == 0)
                        longList[i][0] = 1;
                    if(longList[i][1] == 0)
                        longList[i][1] = 1;
                    if(longList[i][2] == 0)
                        longList[i][2] = 1;
                    if(longList[i][3] == 0)
                        longList[i][3] = 1;
                    totalValues += longList[i][0] * longList[i][1] * longList[i][2] * longList[i][3];
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
