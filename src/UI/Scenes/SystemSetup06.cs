using Godot;
using System.Threading.Tasks;
using UI.Components;

public partial class SystemSetup06 : SystemMenuController
{
	[Export] private CustomOptionButton CursorColorDropdown { get; set; }
	
	public override void _Ready()
	{
		base._Ready();
		CursorColorDropdown.ItemSelected += _onCursorColorItemSelected;
	}

	private void _onCursorColorItemSelected(long id)
	{
		string selectedText = CursorColorDropdown.GetItemText((int)id);
		Cursor.Instance.ReplaceColor(selectedText);
	}
}
