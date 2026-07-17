using System;
using Godot;

/// <summary> handles debug-related functionality with an easy off-switch. </summary>
public partial class Debug : Node
{
    [Export]
    public bool IsEnabled { get; set; } = true;
    private Graphics graphics;

    public override void _Input(InputEvent @event)
    {
        if (!IsEnabled)
        {
            return;
        }

        // note: i don't know if this is the best way to do a global singleton
        // in c#...?
        graphics ??= GetNode<Graphics>("/root/Graphics");

        if (@event.IsActionPressed("debug_quit"))
        {
            GetTree().Quit();
        }

        if (@event.IsActionPressed("debug_increase_window_size"))
        {
            graphics.SetWindowScale(graphics.WindowScale + 1);
        }

        if (@event.IsActionPressed("debug_decrease_window_size"))
        {
            graphics.SetWindowScale(graphics.WindowScale - 1);
        }
    }
}
