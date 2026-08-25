using Godot;
using System;
using System.Threading.Tasks;
using Core.Managers;
using UI.Components;

public partial class SystemSetup01 : Control
{
	private bool _debounce = false;
	
	public override async void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("mouse_left") && !_debounce)
		{
			_debounce = true;
			SoundManager.Instance.PlaySound("ButtonDecide");
			await SceneManager.Instance.ChangeScreenFadeScene(1f, "res://Scenes/SystemSetup02.tscn");
		}
	}
}
