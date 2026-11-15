using Godot;

namespace WSMR.VFX;

public partial class ChannelHover : Node
{
    [Export] public AnimationPlayer AnimPlayer { get; set; }

    public override void _Ready()
    {
        if (AnimPlayer == null)
            return;

        AnimPlayer.AnimationFinished += OnAnimFinished;
        AnimPlayer.Play("Show");
    }

    public override void _ExitTree()
    {
        if (GodotObject.IsInstanceValid(AnimPlayer))
        {
            AnimPlayer.AnimationFinished -= OnAnimFinished;
        }
    }

    private void OnAnimFinished(StringName animName)
    {
        if (animName == "Hide")
        {
            QueueFree();
        }
    }
}