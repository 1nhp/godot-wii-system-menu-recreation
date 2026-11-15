using Core.Managers;
using Godot;
using WSMR.Core.Managers;
using WSMR.UI.Components;

namespace UI.Components;

[GlobalClass]
public partial class CustomButton : PopupButtonHandler
{
    private ButtonAnimator _animator;

    [Export]
    public string ClickSound { get; set; } = "ButtonDecide";
    [Export]
    public int DebounceDelay = 1;
    [Export]
    public bool HasBeenPressed;
    
    [Signal]
    public delegate void ClickedEventHandler();
    
    public override void _Ready()
    {
        _animator = new ButtonAnimator(this);

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        MouseEntered -= OnMouseEntered;
        MouseExited -= OnMouseExited;
        Pressed -= OnPressed;
        base._ExitTree();
    }

    private void OnMouseEntered()
    {
        if (HasBeenPressed)
            return;
        
        SoundManager.Instance.PlaySound("ButtonHover");
        _animator.PlayAnim(ButtonAnimator.Animations.Hover);
    }

    private void OnMouseExited()
    {
        _animator.PlayAnim(ButtonAnimator.Animations.HoverOut);
        base.OnMouseExited();
    }

    private async void OnPressed()
    {
        if (HasBeenPressed)
            return;

        HasBeenPressed = true;
        
        EmitSignal(SignalName.Clicked);
        SoundManager.Instance.PlaySound(ClickSound);
        _animator.PlayAnim(ButtonAnimator.Animations.Click);
        
        await ToSignal(
            GetTree().CreateTimer(DebounceDelay),
            SceneTreeTimer.SignalName.Timeout
        );
        HasBeenPressed = false;
    }
}