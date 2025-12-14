using Godot;
using System;
using System.Text.RegularExpressions;

public partial class TryingRegEx : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Regex regex = new Regex(@"\d+"); 
        MatchCollection matches = regex.Matches("{13,21,35,4}");
        if (matches[0].Success)
        {
            foreach(Match match in matches)
            {
                GD.Print(match.Value.ToInt());
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
