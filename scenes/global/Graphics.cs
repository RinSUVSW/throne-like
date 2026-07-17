using Godot;
using System;

/// <summary>
/// manages global graphic state, such as window size.
/// </summary>
public partial class Graphics : Node2D
{
	public int WindowScale { get; private set; } = 1;
	private readonly Vector2I viewportSize = new Vector2I(256, 192);
	private Window window;

	public override void _Ready()
	{
		window = GetWindow();
		SetWindowScale(3);
	}

	public void SetWindowScale(int scale)
	{
		WindowScale = Mathf.Max(1, scale);
		window.Size = (Vector2I)GetViewportRect().Size * WindowScale;
		CenterWindow();
	}

	private void CenterWindow()
	{
		int screen = DisplayServer.GetKeyboardFocusScreen();
		Vector2I screenPos = DisplayServer.ScreenGetPosition(screen);
		Vector2I screenSize = DisplayServer.ScreenGetSize(screen);
		window.Position = screenPos + (screenSize / 2) - (window.Size / 2);
	}
}
