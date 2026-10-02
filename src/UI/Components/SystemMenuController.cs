using Godot;

namespace UI.Components;

public partial class SystemMenuController : Control
{
    [Export] private Button BackButton;
    [Export] private Button ConfirmButton;

    [Export(PropertyHint.File, "*.tscn")]
    private string BackScene;

    [Export(PropertyHint.File, "*.tscn")]
    private string ConfirmScene;

    public override void _Ready()
    {
        BackButton.Pressed += GoBack;
        ConfirmButton.Pressed += GoForward;
    }

    public async void GoForward()
    {
        DisableInput();
        await SceneManager.Instance.ChangeScreenFadeScene(0.5f, ConfirmScene);
    }

    public async void GoBack()
    {
        DisableInput();
        await SceneManager.Instance.ChangeScreenFadeScene(0.5f, BackScene);
    }

    private void DisableInput()
    {
        MouseBehaviorRecursive = MouseBehaviorRecursiveEnum.Disabled;
    }
}