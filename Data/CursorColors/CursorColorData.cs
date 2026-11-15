using Godot;

[GlobalClass]
public partial class CursorColorData : Resource
{
	[Export] public GradientTexture2D Gradient { get; set; }
	[Export] public Color ReplacementColor { get; set; }
	[Export] public Color TextColor { get; set; } = Colors.White;
}
