using Core.Managers;
using Godot;

namespace UI.Components;

public partial class CustomEntryField : LineEdit
{
    public override void _Ready()
    {
        TextChanged += _onTextChanged;
    }

    private void _onTextChanged(string content = "")
    {
        SoundManager.Instance.PlaySound("KeyboardType");
    }
}
