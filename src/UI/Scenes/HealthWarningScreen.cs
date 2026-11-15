using Godot;
using System.Threading.Tasks;
using Core.Managers;
using UI.Components;

public partial class HealthWarningScreen : Node
{
	[Export] private RichTextLabel ContinueTextLabel { get; set; }
	private Tween _tween;
	private bool _canContinue = false;
	private bool _debounce = true;

	public async override void _Ready()
	{
		await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
		_tween = CreateTween();
		_tween.TweenProperty(ContinueTextLabel, "modulate:a", 1, 0.4);
		_canContinue = true;
	}

	public override void _Input(InputEvent @event)
	{
		if ((@event.IsActionPressed("mouse_left") || @event.IsActionPressed("ui_accept")) && _canContinue && _debounce)
		{
			_debounce = false;
			SoundManager.Instance.PlaySound("Click");
			ContinueTextLabel.Visible = false;
			SceneManager.Instance.ChangeFadeScene(0,"res://Scenes/SystemMenu.tscn", 4);
		}
	}
}
