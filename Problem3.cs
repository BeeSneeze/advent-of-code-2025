using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Problem3 : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var unparsed = LoadFromFile();
        var parsed = ParseData(unparsed);

        long totalPower = 0;


        foreach(var batteryString in parsed)
        {
            List<long> batteryList = new List<long>();
            GD.Print("");

            foreach(var battery in batteryString)
            {
                // This is hilarious
                if(battery > 47 && battery < 58)
                {
                    batteryList.Add(int.Parse(battery.ToString()));
                }                
            }

            long[] bestNumbers = new long[12];
            int[] bestIndices = new int[12];

            long joltSize = 12;

            for(int k = 0; k < joltSize; k++)
            {
                for(int i = 9; i > 0; i--)
                {
                    var checkedIndex = batteryList.FindIndex(0, x => x == i);

                    // Never check the last in the list for the first number
                    if(batteryList.Contains(i) && checkedIndex < batteryList.Count()-(joltSize-1-k))
                    {
                        bestNumbers[k] = i;
                        bestIndices[k] = checkedIndex;
                        break;
                    }
                }

                batteryList = batteryList.GetRange(bestIndices[k]+1,batteryList.Count()-bestIndices[k]-1);
            }

            long addedPower = 0;

            for(int n = 0; n < joltSize; n++)
            {
                addedPower += bestNumbers[n] * (long)Math.Pow(10,(joltSize-n-1));
            }

            GD.Print(addedPower);

            totalPower += addedPower;

        }

        GD.Print(totalPower);

    }

    private string[] ParseData(string unparsed)
    {
        var parsedData = unparsed.Split('\n');
        return parsedData;
    }

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_3.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }
}
