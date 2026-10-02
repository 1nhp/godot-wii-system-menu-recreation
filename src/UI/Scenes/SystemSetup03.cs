using Godot;
using System.Threading.Tasks;
using UI.Components;

public partial class SystemSetup03 : SystemMenuController
{
	[Export] Control Buttons  { get; set; }

	[Export] private SelectionHighlight _highlight;
	
	public override void _Ready()
	{
		base._Ready();
		
		foreach (var child in Buttons.GetChildren())
		{
			if (child is Button button)
			{
				button.Pressed += () => _highlight.Highlight(button);
			}
		}
	}
}
