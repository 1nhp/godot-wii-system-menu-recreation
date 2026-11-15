using Godot;
using Godot.Collections;
[GlobalClass]
public partial class CursorColorsDB : Resource
{
    [Export]
    public Dictionary<string, CursorColorData> ColorsDatabase { get; set; } = new();

    public CursorColorData this[string key]
    {
        get => ColorsDatabase.TryGetValue(key, out var color) ? color : null;
    }
}