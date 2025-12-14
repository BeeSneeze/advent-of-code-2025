using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

public partial class Problem10Copy : Control
{

    private long runTime = 0L;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var data = ParseData(LoadFromFile("res://problem_10.txt"));
        Regex numberRegex = new Regex(@"\d+");

        var inidicatorList = new List<bool[]>();
        var buttonRowList = new List<List<int[]>>();
        var joltageList = new List<int[]>();

        foreach(var row in data)
        {
            var dataArray = row.Split(" ");
            // First the indicators
            var indicatorArray = new bool[dataArray[0].Length-2];
            for(int k = 1; k < dataArray[0].Length-1; k++)
            {
                indicatorArray[k-1] = dataArray[0][k] == '#';
            }
            inidicatorList.Add(indicatorArray);
            // Then the buttons
            var buttons = new List<int[]>();
            for(int n = 1; n < dataArray.Length-1; n++)
            {
                MatchCollection buttonIndices = numberRegex.Matches(dataArray[n]);
                var buttonArray = new int[buttonIndices.Count];
                for(int k = 0; k < buttonIndices.Count; k++)
                {
                    buttonArray[k] = int.Parse(buttonIndices[k].Value);
                    //GD.Print(int.Parse(buttonIndices[k].Value));
                }
                buttons.Add(buttonArray);
            }
            buttonRowList.Add(buttons);
            // And finally the joltage requirements
            MatchCollection joltageNumbers = numberRegex.Matches(dataArray.Last());
            var joltageArray = new int[joltageNumbers.Count];
            for(int k = 0; k < joltageNumbers.Count; k++)
            {
                joltageArray[k] = int.Parse(joltageNumbers[k].Value);
            }
            joltageList.Add(joltageArray);
        }

        var bestToggleNumbers = 0;

        for(int k = 0; k < inidicatorList.Count; k++)
        {
            var indicator = inidicatorList[k];
            var buttonList = buttonRowList[k];

            //GD.Print(indicator.Select(x => x.ToString()).Aggregate("", (x,y) => x + y));
            
            currentBest = 100000;
            var lightArray = new bool[indicator.Length];
            var activeButtons = new bool[buttonList.Count];
            bestToggleNumbers += AmountOfTogglesNeeded(lightArray, indicator, 0, buttonList, activeButtons);
        }

        GD.Print(bestToggleNumbers);
        GD.Print(runTime);

    }

    private int currentBest = 0;

    private int AmountOfTogglesNeeded(bool[] lightArray, bool[] indicatorArray, int depth, List<int[]> buttonList, bool[] activeButtons)
    {
        runTime++;

        if(currentBest <= depth)
        {
            return depth;
        }

        var allEqual = true;
        for(int k=0; k < lightArray.Length; k++)
        {
            if(lightArray[k] != indicatorArray[k])
            {
                allEqual = false;
            }
        }

        if(allEqual)
        {
            return depth;
        }


        // Never look back! BA is useless since we already tested AB
        var earliestButtonIndex = 0;
        for(int a = 0; a < buttonList.Count; a++)
        {
            if(activeButtons[a])
            {
                earliestButtonIndex = a;
                break;
            }
        }

        var bestDepth = 1000000;
        for(int i = earliestButtonIndex; i < buttonList.Count; i++)
        {
            if(activeButtons[i])
            {
                continue; // Don't reactivate buttons, that will never help
            }
            var newActive = (bool[])activeButtons.Clone();
            newActive[i] = true;
            var checkedDepth = AmountOfTogglesNeeded(ToggleLights(buttonList[i],lightArray), indicatorArray, depth + 1, buttonList, newActive);
            if(checkedDepth < bestDepth)
            {
                bestDepth = checkedDepth;
                if(bestDepth < currentBest)
                {
                    currentBest = bestDepth;
                }
            }
        }
        return bestDepth;
    }

    private bool[] ToggleLights(int[] pressedButton, bool[] lightArray)
    {
        var outArray = (bool[])lightArray.Clone();
        foreach(var index in pressedButton)
        {
            outArray[index] = !lightArray[index];
        }
        return outArray;
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
