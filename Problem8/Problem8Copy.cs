using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class Problem8Copy : Control
{

    private float CalculateDistance(Vector3I a, Vector3I b)
    {
        return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var data = ParseData(LoadFromFile("res://problem_8_example.txt"));
        var boxList = data.Select(x => new Vector3I(x.Split(',')[0].ToInt(),x.Split(',')[1].ToInt(),x.Split(',')[2].ToInt())).ToArray();
        GD.Print(data.Length);
        GD.Print(boxList.Length);

        // Perform the distance calculations only once
        var distanceMatrix = new float[boxList.Length,boxList.Length]; // There are (n)^2 possible distance comparisons
        var connections = new HashSet<List<int>>();
        for(int k = 0; k < boxList.Length; k++)
        {
            var newBucket = new List<int>();
            newBucket.Add(k);
            connections.Add(newBucket);
        }

        for(int x = 0; x < boxList.Length; x++)
        {
            for(int y = 0; y < boxList.Length; y++)
            {
                distanceMatrix[x,y] = CalculateDistance(boxList[x], boxList[y]);
                if(x == y)
                {
                    distanceMatrix[x,y] = 1000000000.0f; // Never try and connect the same box to itself
                }
            }
        }

        var allowedConnections = 10;
        while(allowedConnections > 0)
        {
            var res = GetLowestPair(distanceMatrix, boxList.Length);

            if(connections.Any(x => x.Contains(res.a) && x.Contains(res.b)))
            {
                // Found a pair in the same bucket, mark as infinite distance, and try again
                distanceMatrix[res.a,res.b] = 100000000000.0f;
                distanceMatrix[res.b,res.a] = 100000000000.0f;
                allowedConnections--;
            }
            else
            {
                // Mark them as part of the same group
                var bucketA = connections.First(x => x.Contains(res.a));
                var bucketB = connections.First(x => x.Contains(res.b));
                connections.Remove(bucketA);
                connections.Remove(bucketB);
                var bucketBoth = new List<int>(bucketA.Union(bucketB).ToList());
                connections.Add(bucketBoth);

                distanceMatrix[res.a,res.b] = 100000000000.0f;
                distanceMatrix[res.b,res.a] = 100000000000.0f;
                
                GD.Print("CONNECTION MADE!");
                GD.Print(boxList[res.a]);
                GD.Print(boxList[res.b]);
                allowedConnections--;
            }
        }

        

        var lengthList = connections.Select(x => x.Count).ToList();

        lengthList.Sort();
        lengthList.Reverse();

        long answer = lengthList[0] * lengthList[1] * lengthList[2];
        
        GD.Print(lengthList.Count);
        GD.Print(answer);

        foreach(var item in lengthList)
        {
            GD.Print(item);
        }

        GD.Print(lengthList[0]);
        GD.Print(lengthList[1]);
        GD.Print(lengthList[2]);


    }

    private (int a, int b) GetLowestPair(float [,] distanceMatrix, int size)
    {
        var lowestDistance = 100000000000.0f;
        var lowestX = -1;
        var lowestY = -1;
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if(distanceMatrix[x,y] < lowestDistance)
                {
                    lowestDistance = distanceMatrix[x,y];
                    lowestX = x;
                    lowestY = y;
                }
            }
        }

        GD.Print(lowestDistance);

        return (lowestX, lowestY);
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
