using Godot;

namespace Core.Managers;

public partial class Debug : Node
{
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("restart_scene")) GetTree().ReloadCurrentScene();
		if (@event.IsActionPressed("restart_first_scene")) GetTree().ChangeSceneToFile("res://Scenes/SystemSetup01.tscn");
	}
}
