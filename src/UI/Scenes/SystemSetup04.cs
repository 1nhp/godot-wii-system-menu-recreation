using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Components;
using Utils;

public partial class SystemSetup04 : Control
{
	[Export] public Button ConfirmButton { get; set; }
	[Export] public Label HoverText { get; set; }

	[Export] public CustomCheckButton VsyncCheckButton { get; set; }
	[Export] public CustomCheckButton FullScreenCheckButton { get; set; }
	[Export] public CustomCheckButton ShowFrameRateCheckButton { get; set; }
	[Export] public CustomOptionButton SpriteResolutionDropdown { get; set; }
	[Export] public CustomOptionButton ScreenResolutionDropdown { get; set; }

	private readonly Dictionary<Control, (Action entered, Action exited)> _hoverHandlers = new();

	private static readonly Vector2I[] ScreenResolutions =
	{
		new Vector2I(800, 600),
		new Vector2I(1024, 600),
		new Vector2I(1280, 1024),
		new Vector2I(1280, 720),
		new Vector2I(1366, 768),
		new Vector2I(1440, 900),
		new Vector2I(1600, 900),
		new Vector2I(1920, 1080),
		new Vector2I(2560, 1440),
		new Vector2I(3840, 2160),
	};

	public override void _Ready()
	{
		ConfirmButton.Pressed += _onConfirmButtonPressed;

		VsyncCheckButton.Toggled += _onVsyncCheckButtonToggled;
		FullScreenCheckButton.Toggled += _onFullScreenCheckButtonToggled;
		ShowFrameRateCheckButton.Toggled += _onFrameRateCheckButtonToggled;

		RegisterHover(
			VsyncCheckButton,
            "Reduces screen tearing at the cost of latency."
		);

		RegisterHover(
			FullScreenCheckButton,
            "Runs the program in fullscreen mode."
		);

		RegisterHover(
			ShowFrameRateCheckButton,
            "Shows the Framerate in Top-Left corner."
		);

		SpriteResolutionDropdown.ItemSelected += _onSpriteResolutionDropdownItemSelected;
		ScreenResolutionDropdown.ItemSelected += _onScreenResolutionDropdownItemSelected;
	}

	public bool ScreenResolutionWasSet = false;
	
	private void _onScreenResolutionDropdownItemSelected(long id)
	{
		var notification =
			ObjectHelper.Create<Notification>("Notification", Vector2.Zero, GetTree().CurrentScene.GetPath());
		notification.Message = "Are you sure you want to change the screen resolution?\n\n" +
		                       "doing this might result in a `No Signal` screen on some monitors continue? ";
		notification._ConfirmButtonText = "Set resolution";

		int selectedId = (int)id;
		SignalBus.NotificationFinishedEventHandler onFinished = null;
		onFinished = async response =>
		{
			if (!response) return;
			ApplyScreenResolution(selectedId);
		};
		SignalBus.Instance.NotificationFinished += onFinished;

		notification.UpdateUI();
	}

	private async void ApplyScreenResolution(int id)
	{
		DisplayServer.WindowSetSize(ScreenResolutions[id]);
		GetWindow().Size = ScreenResolutions[id];
	}
	
	private void _onSpriteResolutionDropdownItemSelected(long id)
	{
		switch (id)
		{
		 case 0:
			 ResolutionManager.Instance.SetResolution("");
			 break;
		 case 1:
			 ResolutionManager.Instance.SetResolution("_720");
			 break; 
		 case 2:
			 ResolutionManager.Instance.SetResolution("_1080");
			 break;
		 case 3:
			 ResolutionManager.Instance.SetResolution("_4K");
			 break;
		}
	}
	
	public override void _ExitTree()
	{
		ConfirmButton.Pressed -= _onConfirmButtonPressed;

		VsyncCheckButton.Toggled -= _onVsyncCheckButtonToggled;
		FullScreenCheckButton.Toggled -= _onFullScreenCheckButtonToggled;
		ShowFrameRateCheckButton.Toggled -= _onFrameRateCheckButtonToggled;

		SpriteResolutionDropdown.ItemSelected -= _onSpriteResolutionDropdownItemSelected;
		ScreenResolutionDropdown.ItemSelected -= _onScreenResolutionDropdownItemSelected;

		RemoveHover(VsyncCheckButton);
		RemoveHover(FullScreenCheckButton);
		RemoveHover(ShowFrameRateCheckButton);
	}

	private void RegisterHover(Control control, string text)
	{
		Action entered = () => HoverText.Text = text;
		Action exited = () =>
			HoverText.Text = "Move cursor on an option to see details.";

		control.MouseEntered += entered;
		control.MouseExited += exited;

		_hoverHandlers[control] = (entered, exited);
	}

	private void RemoveHover(Control control)
	{
		if (!_hoverHandlers.TryGetValue(control, out var handlers))
			return;

		control.MouseEntered -= handlers.entered;
		control.MouseExited -= handlers.exited;

		_hoverHandlers.Remove(control);
	}

	private void _onVsyncCheckButtonToggled(bool toggled)
	{
		DisplayServer.WindowSetVsyncMode(
			toggled
				? DisplayServer.VSyncMode.Mailbox
				: DisplayServer.VSyncMode.Disabled
		);
	}

	private void _onFullScreenCheckButtonToggled(bool toggled)
	{
		DisplayServer.WindowSetMode(
			toggled
				? DisplayServer.WindowMode.Fullscreen
				: DisplayServer.WindowMode.Windowed
		);
	}

	private void _onFrameRateCheckButtonToggled(bool toggled)
	{
		if (toggled)
			FPSCounter.Instance.Enable();
		else
			FPSCounter.Instance.Disable();
	}

	private async void _onConfirmButtonPressed()
	{
		MouseBehaviorRecursive = MouseBehaviorRecursiveEnum.Disabled;

		await SceneManager.Instance.ChangeScreenFadeScene(
			0.5f,
            "res://Scenes/SystemSetup04.tscn"
		);
	}
}