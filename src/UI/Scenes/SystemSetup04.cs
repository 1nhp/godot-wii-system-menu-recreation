using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Managers;
using UI.Components;
using Utils;
using WSMR.Core.Managers;
using WSMR.Helpers;
using WSMR.UI.Components;
using WSMR.Utils;
using WSMR.VFX;

public partial class SystemSetup04 : SystemMenuController
{
	[Export] private HoverTextController HoverController { get; set; }
	
	[Export] public Label HoverText { get; set; }

	[Export] public CustomCheckButton VsyncCheckButton { get; set; }
	[Export] public CustomCheckButton FullScreenCheckButton { get; set; }
	[Export] public CustomCheckButton ShowFrameRateCheckButton { get; set; }
	[Export] public CustomOptionButton TexturePackDropdown { get; set; }
	[Export] public CustomOptionButton ScreenResolutionDropdown { get; set; }
	[Export] public CustomOptionButton FramerateDropdown { get; set; }

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

	private static readonly string[] SpriteResolutions =
	{
		"",
		"_720",
		"_1080",
		"_4K"
	};
	
	private static readonly int[] Framerates =
	{
		30,
		60,
		90,
		120,
		144,
		240,
	};
	
	public override void _Ready()
	{
		base._Ready();
		
		SettingsManager.Instance.FetchTexturePacks();
		
		int selectedIndex = 0;
		string currentPack =
			SettingsManager.Instance.CurrentSettings.CurrentTexturePack;

		for (int i = 0; i < SettingsManager.Instance.TexturePacksList.Count; i++)
		{
			string texturePack = SettingsManager.Instance.TexturePacksList[i];
			TexturePackDropdown.AddItem(texturePack);
			if (texturePack == currentPack)
				selectedIndex = i;
		}

		if (SettingsManager.Instance.TexturePacksList.Count > 0)
			TexturePackDropdown.Select(selectedIndex);

		
		VsyncCheckButton.Toggled += _onVsyncCheckButtonToggled;
		FullScreenCheckButton.Toggled += _onFullScreenCheckButtonToggled;
		ShowFrameRateCheckButton.Toggled += _onFrameRateCheckButtonToggled;
		
		HoverController.Register(
			VsyncCheckButton,
			"Reduces screen tearing at the cost of latency."
		);

		HoverController.Register(
			FullScreenCheckButton,
			"Runs the program in fullscreen mode."
		);

		HoverController.Register(
			ShowFrameRateCheckButton,
			"Shows the Framerate in Top-Left corner."
		);

		HoverController.Register(
			FramerateDropdown,
			"Change FPS. Lower FPS uses less power but looks less smooth. Higher FPS is smoother but uses more power."
		);

		HoverController.Register(
			TexturePackDropdown,
			"Change sprite resolution. Lower resolutions look blurrier on bigger screens."
		);
		
		TexturePackDropdown.ItemSelected += _onTexturePackDropdownItemSelected;

		//ScreenResolutionDropdown.ItemSelected += _onScreenResolutionDropdownItemSelected;
		FramerateDropdown.ItemSelected += _onFramerateDropdownItemSelected;
	}
	
	// Unused screen resolution option
	//public bool ScreenResolutionWasSet = false;
	//private void _onScreenResolutionDropdownItemSelected(long id)
	//{
	//	var notification =
	//		ObjectHelper.Create<Notification>("Notification", Vector2.Zero, GetTree().CurrentScene.GetPath());
	//	notification.Message = "Are you sure you want to change the screen resolution?\n\n" +
	//						   "doing this might result in a `No Signal` screen on some monitors continue? ";
	//	notification._ConfirmButtonText = "Set resolution";
	//
//		int selectedId = (int)id;
//		SignalBus.NotificationFinishedEventHandler onFinished = null;
//		onFinished = async response =>
//		{
//			if (!response) return;
//			ApplyScreenResolution(selectedId);
//		};
//		SignalBus.Instance.NotificationFinished += onFinished;
//
//		notification.UpdateUI();
//	}

	//private async void ApplyScreenResolution(int id)
	//{
	//	DisplayServer.WindowSetSize(ScreenResolutions[id]);
	//	GetWindow().Size = ScreenResolutions[id];
	//}
	
	private async void _onTexturePackDropdownItemSelected(long id)
	{
		var settings = SettingsManager.Instance;

		settings.CurrentSettings.CurrentTexturePack =
			settings.TexturePacksList[(int)id];

		
		var notification = ObjectHelper.Create<Notification>("Notification", Vector2.Zero, GetTree().CurrentScene.GetPath());
		notification.Message = "In order for the texture pack to change the app has to be restarted continue?";
		notification._ConfirmButtonText = "Restart";
		
		SignalBus.NotificationFinishedEventHandler onFinished = null;
		onFinished = async response =>
		{
			if (!response) return;
			await FadeTransition.Instance.FadeOut(2);
			await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
			RebootProgram.Instance.Reboot();
		};
		
		SignalBus.Instance.NotificationFinished += onFinished;
		settings.SaveSettings();
	}

	private void _onFramerateDropdownItemSelected(long id)
	{
		SettingsManager.Instance.CurrentSettings.FPS = Framerates[id];
		SettingsManager.Instance.CurrentSettings.FrameRateSelected = (int)id;
		SettingsManager.Instance.SaveSettings();
		SettingsManager.Instance.LoadAndApplySettings();
	}
	
	public override void _ExitTree()
	{
		VsyncCheckButton.Toggled -= _onVsyncCheckButtonToggled;
		FullScreenCheckButton.Toggled -= _onFullScreenCheckButtonToggled;
		ShowFrameRateCheckButton.Toggled -= _onFrameRateCheckButtonToggled;
		
		TexturePackDropdown.ItemSelected -= _onTexturePackDropdownItemSelected;
		//ScreenResolutionDropdown.ItemSelected -= _onScreenResolutionDropdownItemSelected;
		FramerateDropdown.ItemSelected -= _onFramerateDropdownItemSelected;
	}

	private void _onVsyncCheckButtonToggled(bool toggled)
	{
		SettingsManager.Instance.CurrentSettings.Vsync = toggled;
		SettingsManager.Instance.SaveSettings();
		SettingsManager.Instance.LoadAndApplySettings();
	}

	private void _onFullScreenCheckButtonToggled(bool toggled)
	{
		SettingsManager.Instance.CurrentSettings.Fullscreen = toggled;
		SettingsManager.Instance.SaveSettings();
		SettingsManager.Instance.LoadAndApplySettings();
	}

	private void _onFrameRateCheckButtonToggled(bool toggled)
	{
		SettingsManager.Instance.CurrentSettings.ShowFramerate = toggled;
		SettingsManager.Instance.SaveSettings();
		SettingsManager.Instance.LoadAndApplySettings();
	}
}
