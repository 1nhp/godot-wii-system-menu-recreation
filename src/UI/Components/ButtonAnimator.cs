using Godot;

namespace UI.Components;

public class ButtonAnimator
{
    private readonly Control _target;
    private readonly Vector2 _buttonScale;

    private Tween _tween;

    public ButtonAnimator(Control target)
    {
        _target = target;
        _buttonScale = target.Scale;
    }

    public enum Animations
    {
        Hover,
        HoverOut,
        Click
    }

    public void PlayAnim(Animations anim)
    {
        //_tween?.Kill();

        _tween = _target.CreateTween();
        _tween.SetTrans(Tween.TransitionType.Quad);

        switch (anim)
        {
            case Animations.Hover:
                _tween.TweenProperty(
                    _target,
                    "offset_transform_scale",
                    new Vector2(
                        _buttonScale.X + 0.1f,
                        _buttonScale.Y + 0.1f
                    ),
                    0.1f
                );
                break;

            case Animations.HoverOut:
                _tween.TweenProperty(
                    _target,
                    "offset_transform_scale",
                    _buttonScale,
                    0.1f
                );
                break;

            case Animations.Click:
                _tween.TweenProperty(
                    _target,
                    "modulate",
                    new Color(3f, 3f, 3f),
                    0
                );

                _tween.TweenProperty(
                    _target,
                    "offset_transform_scale",
                    new Vector2(
                        _buttonScale.X + 0.2f,
                        _buttonScale.Y + 0.2f
                    ),
                    0.06f
                );

                _tween.TweenProperty(
                    _target,
                    "offset_transform_scale",
                    _buttonScale,
                    0.3f
                );

                _tween.Parallel().TweenProperty(
                    _target,
                    "modulate",
                    new Color(1f, 1f, 1f),
                    0.3f
                );
                break;
        }
    }
}