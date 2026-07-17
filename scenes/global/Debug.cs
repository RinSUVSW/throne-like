using System;
using Godot;

/// <summary> handles debug-related functionality with an easy off-switch. </summary>
public partial class Debug : Node
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_quit"))
        {
            GetTree().Quit();
        }
    }
}
