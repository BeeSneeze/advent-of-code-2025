using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Problem1 : Node2D
{
    string unparsedData = "";
    string [] parsedData = [];

    int dialPosition = 50;
    int totalZeroes = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        unparsedData = LoadFromFile();
        ParseData(unparsedData);
        foreach(string item in parsedData)
        {
            int turnAmount = item.Substring(1).ToInt();

            // Add extra turns
            if(turnAmount >= 100)
            {
                int extraTurns = (int)(((float)item.Substring(1).ToInt()) / 100.0f);
                totalZeroes += extraTurns;
                turnAmount %= 100;
                if(turnAmount < 0)
                {
                    turnAmount += 100;
                }
            }

            // Avoid double counting with for example 500
            if(turnAmount == 0)
                continue;

            bool startAtZero = dialPosition == 0;
            
            var addOrSubtract = (item[0] == 'R') ? 1 : -1;

            if(item[0] == 'R')
            {
                dialPosition += turnAmount;
            }
            else
            {
                dialPosition -= turnAmount;
            }

            if(dialPosition < 0)
            {
                if(!startAtZero)
                    totalZeroes += 1; // PASS ZERO

                dialPosition += 100;
            }
            else if(dialPosition >= 100)
            {
                if(dialPosition != 100 && !startAtZero)
                    totalZeroes += 1; // PASS ZERO

                dialPosition -= 100;
            }

            if(dialPosition == 0)
                totalZeroes += 1; // LAND ON ZERO

            if(dialPosition < 0 || dialPosition >= 100)
                throw new Exception("Dial position is in the wrong interval");
        }

        GD.Print(totalZeroes);
    }

    private string[] ParseData(string unparsed)
    {
        parsedData = unparsed.Split('\n');
        return parsedData;
    }

    public string LoadFromFile()
    {
        using var file = FileAccess.Open("res://problem_1.txt", FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        return content;
    }
}
