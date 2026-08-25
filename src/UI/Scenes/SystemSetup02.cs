using Godot;
using System.Threading.Tasks;

public partial class SystemSetup02 : Control
{

    public async override void _Ready()
    {
        await SceneManager.Instance.ChangeScreenFadeScene(3f, "res://Scenes/SystemSetup03.tscn");
    }
}