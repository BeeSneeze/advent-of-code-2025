using Godot;
using System;
using System.Collections.Generic;

public partial class Problem11 : Control
{

    Dictionary<string, Server> serverList = new Dictionary<string,Server>();

    private long runs = 0;
    private long ignoredChildren = 0;
    private string lastSpecial = "dac";
    
    Dictionary<string, long> earlyCalculations = new Dictionary<string, long>();

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


        // I know that dac is the last one now, for both data sets
        MarkChildrenAsPastLast(serverList["dac"]);
        serverList["dac"].pastLast = false;
        MarkChildrenAsPastFirst(serverList["fft"]);
        serverList["fft"].pastFirst = false;

        GD.Print(GetServerPathAmount(serverList["svr"]));
        GD.Print(FindTrueAmount(serverList["svr"], false));

        GD.Print(runs);
    }

    private void FindFirstSpecial(Server examinedServer)
    {
        if(examinedServer.Name == "fft")
        {
            lastSpecial = "fft";
        }
        foreach(var connection in examinedServer.Connections)
        {
            FindFirstSpecial(serverList[connection]);
        }
    }

    private void MarkChildrenAsPastFirst(Server markedServer)
    {
        if(markedServer.pastLast)
        {
            return;
        }
        markedServer.pastFirst = true;
        foreach(var connection in markedServer.Connections)
        {
            MarkChildrenAsPastFirst(serverList[connection]);
        }
    }

    private void MarkChildrenAsPastLast(Server markedServer)
    {
        markedServer.pastLast = true;
        foreach(var connection in markedServer.Connections)
        {
            MarkChildrenAsPastLast(serverList[connection]);
        }
    }

    private long FindTrueAmount(Server examinedServer, bool fft)
    {
        runs++;
        
        if(examinedServer.Name == "dac")
        {
            if(fft)
            {
                return examinedServer.PathAmount;
            }
            else
            {
                return 0;
            }
        }

        if(examinedServer.Name == "fft")
        {
            fft = true;
        }
        
        if(!fft && examinedServer.pastFirst)
        {
            return 0;
        }

        if(!examinedServer.pastFirst && !examinedServer.pastLast && earlyCalculations.ContainsKey(examinedServer.Name))
        {
            return earlyCalculations[examinedServer.Name];
        }

        var pathAmount = 0L;

        foreach(var connection in examinedServer.Connections)
        {
            var childServer = serverList[connection];
            if(!childServer.pastLast)
            {
                pathAmount += FindTrueAmount(childServer, fft);
            }
        }

        if(!examinedServer.pastFirst && !examinedServer.pastLast && !earlyCalculations.ContainsKey(examinedServer.Name))
        {
            earlyCalculations[examinedServer.Name] = pathAmount;
        }

        return pathAmount;
    }

    private long GetServerPathAmount(Server examinedServer)
    {
        if(examinedServer.Name == "out")
        {
            return 1;
        }

        var pathAmount = 0L;

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
        public long PathAmount = -1;
        public bool pastFirst = false;
        public bool pastLast = false;
    };
}
