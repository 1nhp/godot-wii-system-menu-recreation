using Core.Managers;
using Godot;

namespace UI.Components;

public partial class CustomEntryField : LineEdit
{
    private ComponentAnimator _animator;

    
    private bool _skipNextTypingSound;
    
    public override void _Ready()
    {
        _animator = new ComponentAnimator(this);
        
        GuiInput += OnGuiInput;
        TextChanged += _onTextChanged;
        TextChangeRejected += _onTextRejected;
        TextSubmitted += _onTextSubmitted;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is not InputEventKey key || !key.Pressed || key.Echo)
            return;
        
        if (key.CtrlPressed && key.Keycode == Key.A)
        {
            SoundManager.Instance.PlaySound(
                "Paper",
                1f,
                1.1f,
                true
            );

            return;
        }

        if (key.Keycode == Key.Space && Text.Length < 24)
        {
            _skipNextTypingSound = true;

            SoundManager.Instance.PlaySound(
                "KeyboardSpace",
                1f,
                1.1f,
                true
            );

            return;
        }
    }

    private void _onTextChanged(string newText)
    {
        if (_skipNextTypingSound)
        {
            _skipNextTypingSound = false;
            return;
        }

        SoundManager.Instance.PlaySound(
            "KeyboardType",
            0.9f,
            1.2f,
            true
        );
    }

    private void _onTextRejected(string newText)
    {
        _animator.PlayAnim(ComponentAnimator.Animations.Shake);
        
        SoundManager.Instance.PlaySound(
            "KeyboardCantType",
            0.9f,
            1f,
            true
        );
    }
    
    private void _onTextSubmitted(string newText)
    {
        SoundManager.Instance.PlaySound(
            "Click",
            0.9f,
            1f,
            true
        );
    }
    
}