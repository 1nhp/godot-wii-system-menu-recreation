using System.Threading.Tasks;
using Godot;

namespace UI.Components;

public partial class PopupAnimator : Node
{
	private readonly PopupMenu _popup;
	private readonly PanelContainer _target;
	private readonly Vector2 _OriginalPopupSize;
	
	private Tween _tween;

	public PopupAnimator(PanelContainer target, PopupMenu popup)
	{
		_popup = popup;
		_target = target;
		_OriginalPopupSize = target.Scale;
	}

	public enum Animations
	{
		In,
		Out,
	}

	public async Task PlayAnim(Animations anim)
	{
		_tween?.Kill();
		_tween = _target.CreateTween();
		_tween.SetTrans(Tween.TransitionType.Quad);
		
		switch (anim)
		{
			case Animations.In:
				_target.Show();
				_tween.SetEase(Tween.EaseType.Out);
				_target.Scale = Vector2.Zero;
				_target.SetModulate(new Color(1, 1, 1, 0));
				
				_tween.TweenProperty(
					_target, 
					"scale", _OriginalPopupSize, 
					0.2);
				_tween.Parallel().TweenProperty(_target, "modulate:a", 1, 0.25);
				
				break;
			case Animations.Out:
				_tween.SetEase(Tween.EaseType.In);
				_tween.TweenProperty(_target, "scale", Vector2.Zero, 0.25);
				_tween.Parallel().TweenProperty(_target, "modulate:a", 0, 0.15);
				
				await ToSignal(_tween, Tween.SignalName.Finished);
				_popup.Hide();

				break;
		}
	}
}
