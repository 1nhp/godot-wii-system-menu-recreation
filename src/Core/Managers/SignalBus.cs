using Core.Managers;
using Godot;

public partial class SignalBus : Node
{
    public static SignalBus Instance { get; private set; }
    
    [Signal] public delegate void NotificationFinishedEventHandler(bool response);

    public override void _EnterTree() { Instance = this; }
}