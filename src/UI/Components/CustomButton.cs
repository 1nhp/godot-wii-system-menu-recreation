using Core.Managers;
using Godot;

namespace UI.Components;

[GlobalClass]
public partial class CustomButton : Button
{
    private ButtonAnimator _animator;

    [Export]
    private string ClickSound { get; set; } = "ButtonDecide";

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
        SoundManager.Instance.PlaySound("ButtonHover");
        _animator.PlayAnim(ButtonAnimator.Animations.Hover);
    }

    private void OnMouseExited()
    {
        _animator.PlayAnim(ButtonAnimator.Animations.HoverOut);
    }

    private void OnPressed()
    {
        SoundManager.Instance.PlaySound(ClickSound);
        _animator.PlayAnim(ButtonAnimator.Animations.Click);
    }
}