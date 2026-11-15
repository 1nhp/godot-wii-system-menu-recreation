using System;
using Godot;
namespace WSMR.Helpers;

public partial class RebootProgram : Node
{
    public static RebootProgram Instance { get; set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void Reboot()
    {
        string execPath = OS.GetExecutablePath();
        long pid = OS.CreateProcess(execPath, Array.Empty<string>());

        if (pid > 0)
            GetTree().Quit();
        else
            GD.PushError("Failed to restart application.");
    }
}