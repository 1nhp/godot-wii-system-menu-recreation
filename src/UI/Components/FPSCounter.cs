using Godot;
using System;
using System.Globalization;
using System.Threading.Tasks;

public partial class FPSCounter : Control
{
	public static FPSCounter Instance { get; set; }
	
	[Export] private Label CounterLabel { get; set; }
	[Export] private int RefreshDelay { get; set; } = 1;
	public bool Enabled { get; set; } = false;
	
	public override void _Ready()
	{
		if (Enabled) Enable();
		else Disable();

		Instance = this;
		UpdateFPSLoop();
	}

	public async Task UpdateFPSLoop()
	{
		await ToSignal(GetTree().CreateTimer(RefreshDelay), SceneTreeTimer.SignalName.Timeout);
		CounterLabel.Text = $"{Engine.GetFramesPerSecond().ToString(CultureInfo.InvariantCulture)} FPS";
		UpdateFPSLoop();
	}
	
	public void Enable()
	{
		Visible = true;
		Enabled = true;
	}

	public void Disable()
	{
		Visible = false;
		Enabled = false;
	}
}
