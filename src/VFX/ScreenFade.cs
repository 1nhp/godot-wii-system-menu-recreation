using System.Threading.Tasks;
using Godot;
using UI.Components;

public partial class ScreenFade : Control
{
	public static ScreenFade Instance {get; set;}
	[Export] private TextureRect Screen { get; set; }
	
	public Tween _fadeTween;

	public override void _Ready()
	{
		Instance = this;
	}
	
	public async Task FadeOut()
	{
		Cursor.Instance.Hide();
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		var image = GetViewport().GetTexture().GetImage();
		Screen.Texture = ImageTexture.CreateFromImage(image);
		Cursor.Instance.Show();
		
		Screen.Modulate = Colors.White;

		_fadeTween = CreateTween();
		_fadeTween.TweenProperty(Screen, "modulate:a", 0f, 0.5f);
	}
}
