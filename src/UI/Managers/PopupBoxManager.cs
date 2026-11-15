using System.Collections.Generic;
using Godot;
using Utils;
using WSMR.UI.Components;

namespace WSMR.Core.Managers;

public partial class PopupBoxManager : Node
{
    public static PopupBoxManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
    
    public PopupBox _popupBoxNode;

    public void Create(Vector2 position = default, string popupText = "TEST")
    {
        if (IsInstanceValid(_popupBoxNode)) return;
        
        _popupBoxNode = ObjectHelper.Create<PopupBox>(
            "Popup",
            position,
            GetTree().CurrentScene.GetPath()
        );
        _popupBoxNode.PopupText = popupText;
        _popupBoxNode.Update();
    }

    public void Remove()
    {
        if (!IsInstanceValid(_popupBoxNode)) return;

        _popupBoxNode.AnimPlayer.Play("Hide");
        _popupBoxNode = null;
    }
}