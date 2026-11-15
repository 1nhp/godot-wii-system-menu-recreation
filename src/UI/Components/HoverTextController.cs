using Godot;
using System;
using System.Collections.Generic;

namespace UI.Components;

public partial class HoverTextController : Node
{
    [Export] private Label _hoverText;

    [Export]
    private string _defaultText = "Move cursor on an option to see details.";
    private readonly Dictionary<Control, (Action entered, Action exited)> _handlers = new();

    public override void _Ready()
    {
        _hoverText.Text = _defaultText;
    }

    public void Register(Control control, string text)
    {
        if (control == null)
            return;

        Remove(control);

        Action entered = () => _hoverText.Text = text;
        Action exited = () => _hoverText.Text = _defaultText;

        control.MouseEntered += entered;
        control.MouseExited += exited;

        _handlers[control] = (entered, exited);
    }

    public void Remove(Control control)
    {
        if (!_handlers.TryGetValue(control, out var handlers))
            return;

        control.MouseEntered -= handlers.entered;
        control.MouseExited -= handlers.exited;

        _handlers.Remove(control);
    }

    public override void _ExitTree()
    {
        foreach (var (control, handlers) in _handlers)
        {
            control.MouseEntered -= handlers.entered;
            control.MouseExited -= handlers.exited;
        }

        _handlers.Clear();
    }
}