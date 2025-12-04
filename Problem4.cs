using Godot;
using System;
using System.Threading;

public partial class Problem4 : Node2D
{

    private int[,] neighborNumMatrix;
    private bool[,] rollMatrix;

    private int maxRow = 140;
    private int maxColumn = 140;
    
            //int maxRow = parsedData[0].Length;
        //int maxColumn = parsedData.Length;


    private int[] neighborIndicesX = {1,0,-1,1,-1, 1, 0,-1};
    private int[] neighborIndicesY = {1,1, 1,0, 0,-1,-1,-1};

    private int totalAccessibleRolls = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var parsedData = ParseData(LoadFromFile());


        rollMatrix = new bool[maxRow,maxColumn];
        neighborNumMatrix = new int[maxRow,maxColumn];

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

        CalculateNeighbors();
        GD.Print(totalAccessibleRolls);

        var res = FindViableRoll();
        var totalRemovals = 0;

        while(res.foundViable)
        {
            totalRemovals++;
            RemoveRoll(res.foundX,res.foundY);
            res = FindViableRoll();
        }

        GD.Print(totalRemovals);

        totalAccessibleRolls = 0;
        CalculateNeighbors();
        GD.Print(totalAccessibleRolls);
    }

    private void RemoveRoll(int a, int b)
    {
        rollMatrix[a,b] = false;

        for(int k = 0; k < 8; k++)
        {
            var xOkay = neighborIndicesX[k] + a >= 0 && neighborIndicesX[k] + a < maxRow;
            var yOkay = neighborIndicesY[k] + b >= 0 && neighborIndicesY[k] + b < maxRow;
            if(xOkay && yOkay)
            {
                neighborNumMatrix[neighborIndicesX[k] + a,neighborIndicesY[k] + b] -= 1;
                if(neighborNumMatrix[neighborIndicesX[k] + a,neighborIndicesY[k] + b] < 0)
                {
                    throw new Exception("TOO MANY NEIGHBOR REMOVALS!");
                }
            }
        }
    }

    private (bool foundViable, int foundX, int foundY) FindViableRoll()
    {
        for(int x = 0; x < maxRow; x++)
        {
            for(int y = 0; y < maxColumn; y++)
            {
                if(rollMatrix[x,y] && neighborNumMatrix[x,y] < 4)
                {
                    return (true, x, y);
                }
            }
        }
        return (false, -1, -1);
    }

    private void CalculateNeighbors()
    {
        for(int x = 0; x < maxRow; x++)
        {
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

                neighborNumMatrix[x,y] = totalNeighbors;
            }
        }
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
