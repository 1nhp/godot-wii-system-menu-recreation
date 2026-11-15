using System;
using Core.Managers;
using Godot;
namespace UI.Components;

public partial class Cursor : Control
{
	[Export] private Label PlayerNumber { get; set; }
	[Export] private Label PlayerNumberGrabbed { get; set; }

	
	[Export] private TextureRect CursorSprite { get; set; }
	[Export] private TextureRect CursorShadowSprite { get; set; }
	
	[Export] private TextureRect CursorGrabbedSprite { get; set; }
	[Export] private TextureRect CursorGrabbedShadowSprite { get; set; }
	[Export] private AnimationTree DanceAnimation { get; set; }
	[Export] private bool Dancing { get; set; }

	[Export] public float CursorWeight { get; set; } = 26f;

	private bool _holding;
	private float _targetRotation;

	public static Cursor Instance { get; set; }

	[Export] public CursorColorsDB Colors { get; set; }
	[Export] public string Color { get; set; }

	public override void _Ready()
	{
		Instance = this;
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		Position = GetViewport().GetMousePosition();
		ReplaceColor(Color);
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
		
		if (@event.IsActionPressed("cursor_dance") && !@event.IsEcho())
		{
			ToggleDance();
		}
	}
	
	private Tween _danceTween;
	
	private void ToggleDance()
	{
		SoundManager.Instance.PlaySound("Click");
		Dancing = !Dancing;

		_danceTween?.Kill();

		_danceTween = CreateTween();
		_danceTween.TweenProperty(
				DanceAnimation,
				"parameters/blend_position",
				Dancing ? 1f : -1f,
				0.25f
			).SetTrans(Tween.TransitionType.Quad)
			.SetEase(Tween.EaseType.InOut);	
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

	
	public void ReplaceColor(string color)
	{
		var colorData = Colors[color];

		SetCursorShader(CursorSprite, colorData);
		SetCursorShader(CursorGrabbedSprite, colorData);

		PlayerNumber.AddThemeColorOverride("font_color", colorData.TextColor);
		PlayerNumberGrabbed.AddThemeColorOverride("font_color", colorData.TextColor);
	}


	private static void SetCursorShader(TextureRect cursor, CursorColorData color)
	{
		if (cursor.Material is not ShaderMaterial shaderMaterial)
			return;

		shaderMaterial.SetShaderParameter(
			"replacement_color",
			color.ReplacementColor
		);

		shaderMaterial.SetShaderParameter(
			"gradient_texture",
			color.Gradient
		);
	}
}
