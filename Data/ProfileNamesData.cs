using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ProfileNamesData : Resource
{
	[Export]
	public Godot.Collections.Array<string> Names { get; set; } = new();
}
