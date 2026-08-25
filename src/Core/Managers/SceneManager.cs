using System.Threading.Tasks;
using Godot;

public partial class SceneManager : Node
{
    public static SceneManager Instance { get; private set; }

    [Signal]
    public delegate void SceneChangedEventHandler(Node newScene);

    public override void _Ready()
    {
        Instance = this;
    }

    public void ChangeScene(string path)
    {
        var err = GetTree().ChangeSceneToFile(path);
        if (err != Error.Ok)
        {
            GD.PrintErr($"SceneManager: failed to change scene to '{path}' ({err})");
            return;
        }
        _ = NotifyAfterSwap();
    }

    private async Task NotifyAfterSwap()
    {
        while (GetTree().CurrentScene == null)
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        EmitSignal(SignalName.SceneChanged, GetTree().CurrentScene);
    }

    public async Task ChangeScreenFadeScene(float delay, string path)
    {
        await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);

        if (ScreenFade.Instance != null)
            await ScreenFade.Instance.FadeOut();
        else
            GD.PrintErr("SceneManager: ScreenFade.Instance is null, skipping fade.");

        ChangeScene(path);
    }
}