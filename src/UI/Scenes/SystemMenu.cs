using Core.Managers;
using UI.Components;
using Godot;

namespace WSMR.UI.Scenes;

public partial class SystemMenu : Node
{

	[Export] private CustomButton _BoardButton { get; set;}
	[Export] private CustomButton _WiiMenuButton { get; set; }
	[Export] private Control ChannelsContainer { get; set; }

	[Export] private CanvasLayer _board { get; set; }
	[Export] private CanvasLayer _channels { get; set;}

	[Export] private AnimationPlayer _animPlayer { get; set; }

    private Tween _channelsTween;

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

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("scroll_channels_right"))
        {
            ScrollChannels(770, true);
        }
        if (Input.IsActionPressed("scroll_channels_left"))
        {
            ScrollChannels(-770, false);
        }
    }

    private float _scrollOffset = 0f;
    private bool _scrollDebounce = false;
    private int _maxScrollAmount = 4;
    private int _scrolls;

    private async void ScrollChannels(float offset, bool right)
    {
        if (_scrollDebounce) return;
        if (_channels.ProcessMode == ProcessModeEnum.Disabled) return;
        GD.Print(_scrolls);

        if(right){
            if (_scrolls >= _maxScrollAmount) return;
            _scrolls += 1;
            _animPlayer.SpeedScale = 0.9f;
            _animPlayer.Play("Scrolling");

        } else{
            if (_scrolls <= 0) return;
            _scrolls -= 1;
            _animPlayer.SpeedScale = 0.9f;
            _animPlayer.PlayBackwards("Scrolling");
        }

        _scrollDebounce = true;
        _board.ProcessMode = ProcessModeEnum.Disabled;
        SoundManager.Instance.PlaySound("ScrollChannels");

        _channelsTween?.Kill();
        _scrollOffset -= offset;

        _channelsTween = CreateTween();
        _channelsTween.SetTrans(Tween.TransitionType.Quad);
        _channelsTween.TweenProperty(
            ChannelsContainer,
            "position:x",
            _scrollOffset,
            0.3f
        );

        await ToSignal(_channelsTween, Tween.SignalName.Finished);
        _scrollDebounce = false;
        _board.ProcessMode = ProcessModeEnum.Always;
        _animPlayer.SpeedScale = 1f;
    }
}
