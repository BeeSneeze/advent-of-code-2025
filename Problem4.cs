using Godot;
using System;

public partial class Problem4 : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var parsedData = ParseData(LoadFromFile());

        int maxRow = parsedData[0].Length;
        int maxColumn = parsedData.Length;

        bool[,] rollMatrix = new bool[maxRow,maxColumn];
        int[,] neighborNumMatrix = new int[maxRow,maxColumn];

        int totalAccessibleRolls = 0;

        int searchedRow = 0;
        int searchedColumn = 0;
        foreach(var row in parsedData)
        {
            searchedColumn = 0;
            foreach(var column in row)
            {
                rollMatrix[searchedRow,searchedColumn] = column == '@';
                searchedColumn++;
            }
            searchedRow++;
        }


        int[] neighborIndicesX = {1,0,-1,1,-1, 1, 0,-1};
        int[] neighborIndicesY = {1,1, 1,0, 0,-1,-1,-1};

        for(int x = 0; x < maxRow; x++)
        {
            GD.Print("");
            for(int y = 0; y < maxColumn; y++)
            {
                int totalNeighbors = 0;

                for(int k = 0; k < 8; k++)
                {
                    var xOkay = neighborIndicesX[k] + x >= 0 && neighborIndicesX[k] + x < maxRow;
                    var yOkay = neighborIndicesY[k] + y >= 0 && neighborIndicesY[k] + y < maxRow;
                    if(xOkay && yOkay)
                    {
                        if(rollMatrix[neighborIndicesX[k] + x, neighborIndicesY[k] + y])
                        {
                            totalNeighbors++;
                        }
                    }
                }

                if(totalNeighbors < 4 && rollMatrix[x,y])
                {
                    totalAccessibleRolls++;
                }

                GD.Print(totalNeighbors);
                neighborNumMatrix[x,y] = totalNeighbors;
            }
        }
        
        GD.Print(totalAccessibleRolls);
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

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_4.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }
}
