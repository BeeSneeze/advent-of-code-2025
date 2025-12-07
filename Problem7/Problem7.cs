using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class Problem7 : GridContainer
{

    private int amountSplit = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var labRows = ParseData(LoadFromFile("res://Problem7/problem_7.txt"));
        List<long[]> countMatrix = new List<long[]>();
        var countRow = new long[labRows[0].Length]; // This counts the number of possible paths per square on the grid

        for(int k = 1; k < labRows.Length; k++)
        {
            labRows[k] = GetNewRow(labRows[k-1], labRows[k], countRow);
            countMatrix.Add((long[])countRow.Clone());
        }

        GD.Print("Total possible paths: " + countRow.Aggregate(0L, (x,y) => x+y).ToString());
        GD.Print("Total amount of splits: " + amountSplit);
        //VisualizeAnswer(countMatrix);
    }
    
    private string GetNewRow(string oldRow, string newRow, long[] numberRow)
    {
        var outRow = ((string)newRow.Clone()).ToArray(); // Need a char array to write to
        long[] toBeAdded = new long[numberRow.Length]; // First calculate, then write

        for(int i = 0; i < oldRow.Length; i++)
        {
            // Just propagate existing rays
            if(oldRow[i] == '|' && newRow[i] == '.')
                outRow[i] = '|';

            // If it's the starting ray, provide one possible path
            if(oldRow[i] == 'S' && newRow[i] == '.')
            {
                outRow[i] = '|';
                numberRow[i] += 1;
            }

            // If there is a split, transfer number values to new paths
            if(oldRow[i] == '|' && newRow[i] == '^')
            {
                toBeAdded[i] = -numberRow[i]; // There are no rays beneath the splitter
                amountSplit++;
                if(i > 0)
                {
                    outRow[i-1] = '|';
                    toBeAdded[i-1] += numberRow[i];
                }
                if(i + 1 < oldRow.Length)
                {
                    outRow[i+1] = '|';
                    toBeAdded[i+1] += numberRow[i];
                }
            }
        }

        for(int i = 0; i < oldRow.Length; i++)
        {
            numberRow[i] += toBeAdded[i];
        }

        return new string(outRow);
    }

    private void VisualizeAnswer(List<long[]> countMatrix)
    {
        var scene = GD.Load<PackedScene>("res://Problem7/RayGridSquare.tscn");

        this.Columns = countMatrix.Count;
        long highestNumber = 0;

        foreach(var item in countMatrix)
        {
            if(item.Max() > highestNumber)
            {
                highestNumber = item.Max();
            }
        }

        foreach(var column in countMatrix)
        {
            foreach(var number in column)
            {
                var newSquare = scene.Instantiate<ColorRect>();

                if(number == 0)
                {
                    newSquare.Color = new Color(0.2f,0.2f,0.3f);
                }
                else if(number == highestNumber)
                {
                    newSquare.Color = new Color(0,1,0);
                }
                else
                {
                    newSquare.Color = new Color(0,1 + (float)Math.Log((0.001 + (float)number)/(float)highestNumber, 100000),0);
                }

                
                AddChild(newSquare);
                
            }
        }

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
