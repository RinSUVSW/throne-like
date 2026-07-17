using Godot;
using System;
public partial class GunParent : Node2D
{


    [Export] Sprite2D GunSprite;
	[Export] Sprite2D HandSpriteTop;
	[Export] Sprite2D HandSpriteBottom;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if(this.RotationDegrees > 60 && RotationDegrees <= 120)
		{
			GunSprite.ZIndex = -1;
			HandSpriteTop.ZIndex = -1;
			HandSpriteBottom.ZIndex = -2;
		}
        else
        {
            GunSprite.ZIndex = 0;
            HandSpriteTop.ZIndex = 0;
            HandSpriteBottom.ZIndex = -1;
        }
		if (this.RotationDegrees > 100 && RotationDegrees <= 260)
		{
			//GunSprite.FlipV = true;
			Scale = new Vector2(1, -1);
		}
		else
		{
            //GunSprite.FlipV = false;
            Scale = Vector2.One;

        }

    }
}
