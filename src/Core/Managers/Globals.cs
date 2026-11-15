using Godot;

namespace WSMR.Core.Managers;

public partial class Globals : Node
{
    public override void _Ready()
    {
        DirAccess.MakeDirRecursiveAbsolute("user://texture_packs");
        DirAccess.MakeDirRecursiveAbsolute("user://channels");
    }
}