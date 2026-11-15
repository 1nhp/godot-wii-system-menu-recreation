using Core.Managers;
using UI.Components;
using Godot;

namespace WSMR.UI.Scenes;

public partial class SystemMenu : Node
{
	
	[Export] private CustomButton _BoardButton { get; set;}
	[Export] private CustomButton _WiiMenuButton { get; set;}
	
	[Export] private AnimationPlayer _animPlayer { get; set;}
	
	public override void _Ready()
	{
		MusicManager.Instance.PlayMusic("WiiTheme");
		_BoardButton.Clicked += _onBoardButtonPressed;
		_WiiMenuButton.Clicked += _onWiiMenuButtonPressed;
	}

	private void _onBoardButtonPressed()
	{
		_animPlayer.Play("BoardOpen");
	}
	private void _onWiiMenuButtonPressed()
	{
		_animPlayer.PlayBackwards("BoardOpen");
	}
}
