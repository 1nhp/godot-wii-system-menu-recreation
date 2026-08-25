using Godot;
using System;
using System.Threading.Tasks;
using Core.Managers;
using UI.Components;

public partial class Notification : Control
{
	[Export] private AnimationPlayer _animPlayer { get; set; }
	[Export] public string AnimationName { get; set; } = "AnimOpen";
	[Export] public string CloseAnimationName { get; set; } = "AnimOpen";

	[Export] public bool OneButton { get; set; }
	[Export] public bool NoButtons { get; set; }

	[Export(PropertyHint.MultilineText)] public string Message { get; set; }
	[Export] public string _CancelButtonText { get; set; }
	[Export] public string _ConfirmButtonText { get; set; }
	
	[Export] private CustomButton _CancelButton { get; set; }
	[Export] private CustomButton _ConfirmButton { get; set; }
	[Export] private TextureRect _NotificationContainer { get; set; }
	[Export] private FlowContainer _ButtonsContainer { get; set; }

	[Export] private Label _MessageLabel { get; set; }

	public bool Closing;

	private Action _onCancelPressed;
	private Action _onConfirmPressed;

	public override void _Ready()
	{
		_NotificationContainer.SetMouseBehaviorRecursive(MouseBehaviorRecursiveEnum.Disabled);
		
		SoundManager.Instance.PlaySound("NotificationOpen");
		_animPlayer.Play(AnimationName);
		_animPlayer.AnimationFinished += _onAnimationFinished;
		_onCancelPressed = () => _onButtonPressed(false);
		_onConfirmPressed = () => _onButtonPressed(true);

		_CancelButton.Pressed += _onCancelPressed;
		_ConfirmButton.Pressed += _onConfirmPressed;
		UpdateUI();
	}

	public void UpdateUI()
	{
		_MessageLabel.Text = Message;
		_CancelButton.Text = _CancelButtonText;
		_ConfirmButton.Text = _ConfirmButtonText;
		
		if (OneButton) _CancelButton.Visible = false;
		if (NoButtons) _ButtonsContainer.Visible = false;
	}
	
	private void _onAnimationFinished(StringName animName)
	{
		if (!Closing)
		{
			_NotificationContainer.SetMouseBehaviorRecursive(MouseBehaviorRecursiveEnum.Enabled);
		}
		else
		{
			QueueFree();
		}
	}

	public override void _ExitTree()
	{
		_CancelButton.Pressed -= _onCancelPressed;
		_ConfirmButton.Pressed -= _onConfirmPressed;
	}

	private async void _onButtonPressed(bool response)
	{
		_NotificationContainer.SetMouseBehaviorRecursive(MouseBehaviorRecursiveEnum.Disabled);
		await ToSignal(GetTree().CreateTimer(0.2), SceneTreeTimer.SignalName.Timeout);
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.NotificationFinished, response);
		_animPlayer.PlayBackwards(CloseAnimationName);
		Closing = true;
	}
}
