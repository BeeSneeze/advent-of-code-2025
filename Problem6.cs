using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class Problem6 : Node2D
{
    private const int NUMBER_OF_ROWS = 4;
    public override void _Ready()
    {
        var worksheetRows = ParseData(LoadFromFile("res://problem_6.txt"));
        var stringMatrix = worksheetRows[0..NUMBER_OF_ROWS].Select(x=>x.ToArray().Select(c => c.ToString()).ToArray()).ToArray();

        List<List<long>> equationList = new List<List<long>>();
        List<long> savedNumbers = new List<long>();
        for(int k = stringMatrix[0].Length-1; k >= 0; k--)
        {
            if(stringMatrix.All(x => x[k] == " "))
            {
                equationList.Add(savedNumbers);
                savedNumbers = new List<long>();
            }
            else
            {
                var numberString = "";
                for(int n = 0; n < NUMBER_OF_ROWS; n++)
                {
                    numberString += stringMatrix[n][k];
                }
                savedNumbers.Add(long.Parse(numberString));
            }
        }
        equationList.Add(savedNumbers); // Don't forget to add the leftmost calculation!

        var operationString = worksheetRows.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
        long totalValues = 0;
        for(int i = 0; i < operationString.Length; i++)
        {
            switch(operationString[i])
            {
                case "+":
                    long addedNumber = 0;
                    foreach(long num in equationList[i])
                    {
                        addedNumber += num;
                    }
                    totalValues += addedNumber;
                break;
                case "*":
                    long multipliedNumber = 1;
                    foreach(long num in equationList[i])
                    {
                        multipliedNumber *= num;
                    }
                    totalValues += multipliedNumber;
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
