using Godot;
using System;
using System.Globalization;
using System.Threading.Tasks;

public partial class FPSCounter : CanvasLayer
{
	public static FPSCounter Instance { get; set; }
	
	[Export] private Label CounterLabel { get; set; }
	[Export] private int RefreshDelay { get; set; } = 1;
	public bool Enabled { get; set; } = false;
	
	public override void _Ready()
	{
		if (Enabled) Toggle(Enabled);
		else Toggle(Enabled);

		Instance = this;
		UpdateFPSLoop();
	}

	public async Task UpdateFPSLoop()
	{
		await ToSignal(GetTree().CreateTimer(RefreshDelay), SceneTreeTimer.SignalName.Timeout);
		CounterLabel.Text = $"{Engine.GetFramesPerSecond().ToString(CultureInfo.InvariantCulture)} FPS";
		UpdateFPSLoop();
	}
	
	public void Toggle(bool enabled)
	{
		if (enabled)
		{
			Visible = true;
			Enabled = true;
		}
		else
		{
			Visible = false;
			Enabled = false;
		}
	}
}
