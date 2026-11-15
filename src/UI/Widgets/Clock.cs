using Godot;
using System;

public partial class Clock : Node
{
	[Export] public Label DateLabel {get; set;}
	[Export] public RichTextLabel TimeLabel {get; set;}

	public override void _Ready(){
		_UpdateDate();
	}

	public async void _UpdateDate()
	{
		while (true)
		{
			DateTime now = DateTime.Now;

			string formattedDate = now.ToString("MMM d/d");
			string time = now.ToString("h mm");
			string amPm = now.ToString("tt");

			DateLabel.Text = formattedDate;
			TimeLabel.Text = $"{time} [font_size=30]{amPm}[/font_size]";

			await ToSignal(
				GetTree().CreateTimer(60),
				SceneTreeTimer.SignalName.Timeout
			);
		}
	}
}
