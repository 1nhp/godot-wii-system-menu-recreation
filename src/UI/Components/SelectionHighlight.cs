using Godot;
namespace UI.Components;

public partial class SelectionHighlight : Control
{
    [Export] private AnimationPlayer _animationPlayer;

    private Vector2 _targetPosition;
    private Vector2 _targetSize;
    private Control _currentTarget;
    
    
    public override void _Ready()
    {
        Vector2 screenSize = GetViewportRect().Size;
        _targetPosition = screenSize / 2;
    }
    
    public void Highlight(Control target)
    {
        _currentTarget = target;
        var rect = target.GetGlobalRect();

        _targetPosition = new Vector2(rect.Position.X + 10, rect.Position.Y + 5);
        _targetSize = new Vector2(rect.Size.X - 24, rect.Size.Y - 20);

        Visible = true;
        _animationPlayer.Play("Highlight");
    }

    public override void _Process(double delta)
    {
        if (_currentTarget != null)
        {
            var rect = _currentTarget.GetGlobalRect();
            _targetPosition = rect.Position + new Vector2(10, 5);
            _targetSize = rect.Size - new Vector2(24, 20);
        }
        
        GlobalPosition = GlobalPosition.Lerp(_targetPosition, 12f * (float)delta);
        Size = Size.Lerp(_targetSize, 12f * (float)delta);
    }

    public void HideHighlight()
    {
        Visible = false;
    }
}