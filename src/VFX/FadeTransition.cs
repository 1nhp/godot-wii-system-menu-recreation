using System.Threading.Tasks;
using Godot;
namespace WSMR.VFX;
public partial class FadeTransition : CanvasLayer
{
	public static FadeTransition Instance { get; private set; }

	[Export] public AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		Instance = this;
		Visible = false;
	}

	public async Task Fade(float speed = 1)
	{
		_animationPlayer.PlayBackwards("FadeAnim");
		_animationPlayer.SpeedScale = speed;
		
		await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
		_animationPlayer.Play("FadeAnim");
	}

	public async Task FadeOut(float speed = 1)
	{
		_animationPlayer.PlayBackwards("FadeAnim");
		_animationPlayer.SpeedScale = speed;
		await ToSignal(_animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
}
