using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Problem1Copy2 : Node2D
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
            GD.Print("");
            int turnAmount = item.Substring(1).ToInt();
            //GD.Print(turnAmount);

            // Add extra turns
            if(Math.Abs(turnAmount) >= 100)
            {
                //GD.Print("PASSED WITH EXCESSIVE FORCE!!!");
                int extraTurns = (int)(((float)item.Substring(1).ToInt()) / 100.0f);
                //GD.Print(extraTurns);
                totalZeroes += extraTurns;
                //GD.Print(totalZeroes);
                turnAmount %= 100;
                if(turnAmount < 0)
                {
                    turnAmount += 100;
                }
            }

            if(turnAmount < 0 || turnAmount >= 100)
                throw new Exception("FUCK");

            if(turnAmount == 0)
            {
                continue;
            }

            bool startAtZero = false;

            if(dialPosition == 0)
            {
                startAtZero = true;
            }

            if(item[0] == 'R')
            {
                dialPosition += turnAmount;
                //GD.Print("R" + turnAmount.ToString());
            }
            else
            {
                dialPosition -= turnAmount;
                //GD.Print("L" + turnAmount.ToString());
            }

            if(dialPosition < 0)
            {
                if(dialPosition <= -100)
                {
                    throw new Exception("NEVER HAPPENS");
                }
                //GD.Print("PASSED 0");
                if(!startAtZero)
                {
                    totalZeroes += 1; // PASS
                }
                
                //GD.Print(totalZeroes);
                dialPosition += 100;
            }
            else if(dialPosition >= 100)
            {
                if(dialPosition != 100)
                {
                    //GD.Print("PASSED 0");
                    if(!startAtZero)
                    {
                        totalZeroes += 1; // PASS
                    }
                    
                    //GD.Print(totalZeroes);
                }
                dialPosition -= 100;
            }

            if(dialPosition == 0)
            {
                //GD.Print("LANDED ON 0!");
                totalZeroes += 1; // LAND
                //GD.Print(totalZeroes);
            }

            if(dialPosition < 0 || dialPosition >= 100)
                throw new Exception("FUCK");

            
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
