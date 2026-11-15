using System.Reflection;
using Core.Managers;
using Godot;
using UI.Components;
using WSMR.Core.Managers;

namespace WSMR.UI.Components;

public partial class CustomCheckButton : CheckButton
{
	private ButtonAnimator _animator;
	
	[Export]
	private string ClickSound { get; set; } = "Click";
	[Export]
	private string Setting { get; set; } = "Vsync";

	public override void _Ready()
	{
		_animator = new ButtonAnimator(this);

		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		Pressed += OnPressed;

		ButtonPressed = GetSettingValue();
	}

	private bool GetSettingValue()
	{
		var settings = SettingsManager.Instance.CurrentSettings;
		var property = typeof(AppSaveData).GetProperty(
			Setting,
			BindingFlags.Public | BindingFlags.Instance
		);

		return property.GetValue(settings) is bool value && value;
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
		if (ButtonPressed)
		{
			_animator.PlayAnim(ButtonAnimator.Animations.Stretch);
		}
		else
		{
			_animator.PlayAnim(ButtonAnimator.Animations.StretchOut);
		}
	}
}
