using Godot;

namespace UI.Components;

public class ComponentAnimator
{
    private readonly Control _target;
    private readonly Vector2 _originalPosition;

    private Tween? _tween;

    public ComponentAnimator(Control target)
    {
        _target = target;
        _originalPosition = target.Position;
    }

    public enum Animations
    {
        Shake,
    }

    public void PlayAnim(Animations animation)
    {
        _tween?.Kill();

        _tween = _target.CreateTween();

        switch (animation)
        {
            case Animations.Shake:
                PlayShake();
                break;
        }
    }

    private void PlayShake()
    {
        const float distance = 3f;
        const float duration = 0.05f;

        _tween!
            .TweenProperty(
                _target,
                "offset_transform_position",
                _originalPosition + new Vector2(distance, 0),
                duration
            );

        _tween.TweenProperty(
            _target,
            "offset_transform_position",
            _originalPosition - new Vector2(distance, 0),
            duration
        );

        _tween.TweenProperty(
            _target,
            "offset_transform_position",
            _originalPosition + new Vector2(distance, 0),
            duration
        );

        _tween.TweenProperty(
            _target,
            "offset_transform_position",
            _originalPosition,
            duration
        );
    }
}