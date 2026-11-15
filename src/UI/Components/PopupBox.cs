using Core.Managers;
using Godot;

namespace WSMR.UI.Components;

public partial class PopupBox : CanvasLayer
{
    [Export] public AnimationPlayer AnimPlayer { get; set; }
    [Export] public BoxContainer Container { get; set; }
    [Export] public Label Title { get; set; }
    [Export] public string PopupText { get; set; } = "TEST";

    public override void _Ready()
    {
        Update();
        CallDeferred(MethodName._CenterPivot);
        AnimPlayer.Play("Show");
        SoundManager.Instance?.PlaySound("PopupBox");
        AnimPlayer.AnimationFinished += OnAnimFinished;
    }

    public void Update()
    {
        Title.Text = PopupText;
        _CenterPivot();
    }

    public void HidePopup()
    {
        AnimPlayer.Play("Hide");
    }

    private void _CenterPivot()
    {
        Container.PivotOffset = Container.Size / 2.0f;
    }

    public override void _ExitTree()
    {
        AnimPlayer.AnimationFinished -= OnAnimFinished;
    }

    private void OnAnimFinished(StringName animName)
    {
        if (animName == "Hide")
        {
            QueueFree();
        }
    }
}