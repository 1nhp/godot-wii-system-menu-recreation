using Godot;
namespace UI.Components;

public partial class Cursor : Control
{
	[Export] private TextureRect CursorSprite { get; set; }
	[Export] private TextureRect CursorShadowSprite { get; set; }
	
	[Export] private TextureRect CursorGrabbedSprite { get; set; }
	[Export] private TextureRect CursorGrabbedShadowSprite { get; set; }
	
	[Export] public float CursorWeight { get; set; } = 26f;

	private bool _holding;
	private float _targetRotation;

	public static Cursor Instance { get; set; }
	
	public override void _Ready()
	{
		Instance = this;
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		Position = GetViewport().GetMousePosition();
	}

	private Vector2 _mouseDelta;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			_mouseDelta = motion.Relative;
		}
		else
		{
			_mouseDelta = Vector2.Zero;
		}
	}
	
	public override void _Process(double delta)
	{
		Position = Position.Lerp(GetViewport().GetMousePosition(), CursorWeight * (float)delta);
		_holding = Input.IsActionPressed("mouse_left");

		if (_holding)
		{
			CursorGrabbedSprite.Show();
			CursorGrabbedShadowSprite.Show();
			CursorSprite.Hide();
			CursorShadowSprite.Hide();
		}
		else
		{
			CursorGrabbedSprite.Hide();
			CursorGrabbedShadowSprite.Hide();
			CursorSprite.Show();
			CursorShadowSprite.Show();
		}
		
		_targetRotation = _holding
			? Mathf.Clamp(_mouseDelta.X * 0.04f + _mouseDelta.Y * 0.04f, -1f, 10f)
			: 0f;
		Rotation = Mathf.LerpAngle(Rotation, _targetRotation, 12f * (float)delta);
	}
}
