using Godot;
using System.Threading.Tasks;

namespace UI.Components;

public partial class SystemMenuController : Control
{
    [Export] private Button BackButton;
    [Export] private Button ConfirmButton;

    public enum Transitions
    {
        ScreenFade,
        Fade,
    }
    
    [Export] public Transitions SceneAnimation;
    [Export] public float TransitionSpeed { get; set; }
    [Export] public float TransitionDelay { get; set; } = 0.2f;

    [Export(PropertyHint.File, "*.tscn")]
    private string BackScene;

    [Export(PropertyHint.File, "*.tscn")]
    private string ConfirmScene;

    private bool _isTransitioning;

    public override void _Ready()
    {
        BackButton.Pressed += GoBack;
        ConfirmButton.Pressed += GoForward;
    }

    private async void GoForward()
    {
        if (_isTransitioning)
            return;

        _isTransitioning = true;
        DisableInput();

        await ChangeScene(ConfirmScene);
    }

    private async void GoBack()
    {
        if (_isTransitioning)
            return;

        _isTransitioning = true;
        DisableInput();

        await ChangeScene(BackScene);
    }

    private async Task ChangeScene(string scene)
    {
        switch (SceneAnimation)
        {
            case Transitions.ScreenFade:
                await SceneManager.Instance.ChangeScreenFadeScene(TransitionDelay, scene);
                break;

            case Transitions.Fade:
                await SceneManager.Instance.ChangeFadeScene(TransitionDelay, scene, TransitionSpeed);
                break;
        }
    }

    private void DisableInput()
    {
        MouseBehaviorRecursive = MouseBehaviorRecursiveEnum.Disabled;
    }
}