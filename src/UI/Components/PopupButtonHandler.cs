using Godot;
using WSMR.Core.Managers;

namespace WSMR.UI.Components;

public partial class PopupButtonHandler : Button
{
    [Export] private Vector2 _popupOffset;
    private int _hoverTime = 0;
    [Export] public int PopupDelay = 15;
    [Export] public string PopupText = "TEST";
    [Export] public bool ShowPopup;
    
    public override void _Process(double delta)
    {
        if (IsHovered() && ShowPopup)
        {
            if (_hoverTime < PopupDelay)
            {
                _hoverTime += 1;
                GD.Print(_hoverTime);
            }
            else
            {
                PopupBoxManager.Instance.Create(GlobalPosition - _popupOffset, PopupText);
            }
        }
    }

    public void OnMouseExited()
    {
        if (!ShowPopup) return;
        PopupBoxManager.Instance.Remove();
        _hoverTime = 0;
    }
}