using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class Problem11 : Control
{

    Dictionary<string, Server> serverList = new Dictionary<string,Server>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var data = ParseData(LoadFromFile("res://problem_11.txt"));

        foreach(var row in data)
        {
            var res = row.Split(":");
            var connectionList = res[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            serverList.Add(res[0], new Server(res[0], connectionList));
        }
        serverList.Add("out", new Server("out", []));

        GD.Print(GetServerPathAmount(serverList["you"]));


    }

    private int GetServerPathAmount(Server examinedServer)
    {
        if(examinedServer.Name == "out")
        {
            return 1;
        }

        var pathAmount = 0;

        foreach(var connection in examinedServer.Connections)
        {
            if(serverList[connection].PathAmount == -1)
            {
                serverList[connection].PathAmount = GetServerPathAmount(serverList[connection]);
            }

            pathAmount += serverList[connection].PathAmount;
        }

        return pathAmount;
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

    record class Server(string Name, string[] Connections)
    {
        public int PathAmount = -1;
    };
}
