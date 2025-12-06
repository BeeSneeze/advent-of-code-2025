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
        var stringMatrix = worksheetRows[0..NUMBER_OF_ROWS] // A matrix of the numbers expressed as strings (placed in rows and columns)
            .Select(x => x.ToArray()        
                .Select(c => c.ToString())  
                .ToArray())                 
            .ToArray();                     

        List<List<long>> equationList = new List<List<long>>();
        List<long> savedNumbers = new List<long>();
        for(int k = stringMatrix[0].Length-1; k >= 0; k--)
        {
            if(stringMatrix.All(x => x[k] == " "))
            {
                equationList.Add(savedNumbers);
                savedNumbers = new List<long>(); // Make sure it's a new copy, don't just clear the list
            }
            else
            {
                savedNumbers.Add(long.Parse(stringMatrix.Aggregate("", (x,y) => x + y[k])));
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
                    totalValues += equationList[i].Aggregate(0L, (x,y) => x + y);
                break;
                case "*":
                    totalValues += equationList[i].Aggregate(1L, (x,y) => x * y);
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
