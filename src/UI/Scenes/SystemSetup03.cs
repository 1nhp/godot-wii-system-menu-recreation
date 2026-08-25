using Godot;
using System.Threading.Tasks;
using UI.Components;

public partial class SystemSetup03 : Control
{
	[Export] Control Buttons  { get; set; }
	[Export] Button ConfirmButton  { get; set; }
	[Export] Button BackButton  { get; set; }

	[Export] private SelectionHighlight _highlight;
	
	public override void _Ready()
	{
		foreach (var child in Buttons.GetChildren())
		{
			if (child is Button button)
			{
				button.Pressed += () => _highlight.Highlight(button);
			}
		}

		ConfirmButton.Pressed += _onConfirmButtonPressed;
		BackButton.Pressed += _onBackButtonPressed;
	}

	private async void _onConfirmButtonPressed()
	{
		MouseBehaviorRecursive = MouseBehaviorRecursiveEnum.Disabled;
		await SceneManager.Instance.ChangeScreenFadeScene(0.5f, "res://Scenes/SystemSetup04.tscn");
	}
	
	private async void _onBackButtonPressed()
	{
		MouseBehaviorRecursive = MouseBehaviorRecursiveEnum.Disabled;
		await SceneManager.Instance.ChangeScreenFadeScene(0.5f, "res://Scenes/SystemSetup03.tscn");
	}
}
