using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Problem3 : Node2D
{

    private long joltSize = 12;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var unparsedData = LoadFromFile();

        long totalPower = 0;

        foreach(var batteryString in ParseData(unparsedData))
        {
            List<long> batteryList = new List<long>();

            foreach(char battery in batteryString)
            {
                batteryList.Add(int.Parse(battery.ToString()));            
            }

            long[] bestNumbers = new long[12];
            int[] bestIndices = new int[12];

            for(int k = 0; k < joltSize; k++)
            {
                for(int i = 9; i > 0; i--)
                {
                    var checkedIndex = batteryList.FindIndex(0, x => x == i);

                    // Always leave room for more digits at the end,
                    // a number with more digits will always be bigger than one with fewer!
                    if(batteryList.Contains(i) && checkedIndex < batteryList.Count()-(joltSize-1-k))
                    {
                        bestNumbers[k] = i;
                        bestIndices[k] = checkedIndex;
                        break;
                    }
                }

                // Use the remaining part of the list for the next digit
                batteryList = batteryList.GetRange(bestIndices[k]+1,batteryList.Count()-bestIndices[k]-1);
            }

            // Format the selected number correctly
            long addedPower = 0;
            for(int n = 0; n < joltSize; n++)
            {
                addedPower += bestNumbers[n] * (long)Math.Pow(10,(joltSize-n-1));
            }
            totalPower += addedPower;
        }

        GD.Print(totalPower);

    }

    private string[] ParseData(string unparsed)
    {
        var parsedData = unparsed.Split(System.Environment.NewLine);
        return parsedData;
    }

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_3.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }
}
