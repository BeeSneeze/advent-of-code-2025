using Godot;
using System;
using System.Linq;

public partial class Problem7Copy : Node2D
{

    private int amountSplit = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var labRows = ParseData(LoadFromFile("res://problem_7.txt"));

        //var experimentRows = labRows.Select(x => x.ToArray()).ToArray();

        var startRow = labRows[0];

        for(int k = 1; k < labRows.Length; k++)
        {
            labRows[k] = GetNewRow(labRows[k-1], labRows[k]);
            GD.Print(labRows[k]);
        }

        GD.Print(amountSplit);
    }

    private string GetNewRow(string oldRow, string newRow)
    {
        var outRow = ((string)newRow.Clone()).ToArray();

        for(int i = 0; i < oldRow.Length; i++)
        {
            if(oldRow[i] == 'S' && newRow[i] == '.')
            {
                outRow[i] = '|';
            }

            if(oldRow[i] == '|' && newRow[i] == '.')
            {
                outRow[i] = '|';
            }

            if(oldRow[i] == '|' && newRow[i] == '^')
            {
                amountSplit++;
                if(i > 0)
                {
                    if(newRow[i-1] == '.')
                    {
                        outRow[i-1] = '|';
                    }
                    
                }
                if(i + 1 < oldRow.Length)
                {
                    if(newRow[i+1] == '.')
                    {
                        outRow[i+1] = '|';
                    }
                }
            }
        }


        return new string(outRow);
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
