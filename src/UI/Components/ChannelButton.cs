using Core.Managers;
using Godot;
using Utils;
using WSMR.Core.Managers;
using WSMR.UI.Components;
using WSMR.UI.Scenes;
using WSMR.VFX;

namespace UI.Components;

[GlobalClass]
public partial class ChannelButton : PopupButtonHandler
{
	private ButtonAnimator _animator;
	private ChannelHover _channelHover;

	[Export] private string ClickSound;
	
	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseChannelExited;
		Pressed += OnPressed;
	}

	public override void _ExitTree()
	{
		MouseEntered -= OnMouseEntered;
		MouseExited -= OnMouseChannelExited;
		Pressed -= OnPressed;

		if (GodotObject.IsInstanceValid(_channelHover))
		{
			_channelHover.QueueFree();
			_channelHover = null;
		}

		base._ExitTree();
	}

	private void OnMouseEntered()
	{
		SoundManager.Instance.PlaySound("ButtonHover");
		
		if (GodotObject.IsInstanceValid(_channelHover))
		{
			_channelHover.AnimPlayer.Play("Show");
			return;
		}

		_channelHover = ObjectHelper.Create<ChannelHover>(
			"ChannelHover",
			GlobalPosition,
			GetPath()
		);
	}

	private void OnMouseChannelExited()
	{
		if (!GodotObject.IsInstanceValid(_channelHover))
			return;

		_channelHover.AnimPlayer.Play("Hide");
		base.OnMouseExited();
	}

	private void OnPressed()
	{
		SoundManager.Instance.PlaySound(ClickSound);
	}
}
