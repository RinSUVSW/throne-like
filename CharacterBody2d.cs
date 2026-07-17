using System;
using System.Runtime.CompilerServices;
using Godot;

public partial class CharacterBody2d : CharacterBody2D
{
	public const float Speed = 80.0f;
	public const float JumpVelocity = -400.0f;
	private float m_Speed;
	private Vector2 MovementVector;

	//public Vector2 Velocity;
	[Export]
	Node2D ArrowParent;

	[Export]
	Node2D GunParent;
	private Vector2 shootingDirectionReal;
	public float AimSpeed = 0.5f;

	[Export]
	AnimationPlayer ap; //change later

	[Export]
	Camera2D camera;

	[Export]
	Texture2D Down;

	[Export]
	Texture2D Left0;

	[Export]
	Texture2D Left1;

	[Export]
	Texture2D Left2;

	[Export]
	Texture2D Left3;

	[Export]
	Texture2D Up;
	public Vector2 cameraTarget;

	[Export]
	Sprite2D characterSprite;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		bool doSpriteChange = true;
		// Add the gravity.
		//if (!IsOnFloor())
		//{
		//	velocity += GetGravity() * (float)delta;
		//}

		// Handle Jump.
		//if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		//{
		//	velocity.Y = JumpVelocity;
		//}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			//velocity.X = Mathf.Lerp(Velocity.X, direction.X * Speed, 1);
			//         velocity.Y = Mathf.Lerp(Velocity.Y, direction.Y * Speed, 1f);

			MovementVector = direction;
			MovementVector = MovementVector.Normalized();

			m_Speed = Mathf.Lerp(m_Speed, Speed, 0.8f);
			ArrowParent.Visible = true;
			doSpriteChange = true;
		}
		else
		{
			MovementVector = Vector2.Zero;
			//velocity = velocity.Lerp(Vector2.Zero, 0.2f);
			m_Speed = Mathf.Lerp(m_Speed, 0, 0.2f);
			ArrowParent.Visible = false;
		}

		Velocity = Velocity.Lerp(MovementVector * m_Speed, 0.5f);

		ArrowParent.Rotation = (Mathf.Atan2(Velocity.Y, Velocity.X)) + Mathf.DegToRad(90);

		//Velocity = velocity;
		MoveAndSlide();

		cameraTarget = GlobalPosition;

		Vector2 shootingDirection = Input.GetVector("shoot_left", "shoot_right", "shoot_up", "shoot_down");
		if (shootingDirection == Vector2.Zero)
		{
			shootingDirection = Velocity;
		}
		else
		{
			cameraTarget += (shootingDirection.Normalized() * 20);
		}

		shootingDirectionReal = shootingDirectionReal.Lerp(shootingDirection, AimSpeed);

		GunParent.Rotation = Mathf.Atan2(shootingDirectionReal.Y, shootingDirectionReal.X) + Mathf.DegToRad(180);

		//GetViewport().GetCamera2D()

		//ap.Play("RecoilStartFire");

		if (Input.IsActionJustPressed("Fire"))
		{
			ap.Play("RecoilStartFire");
		}

		if (Input.IsActionPressed("Fire"))
		{
			doSpriteChange = true;
		}

		if (doSpriteChange)
		{
			if (Input.IsActionPressed("Fire"))
			{
				ChangeSprite(shootingDirectionReal);
			}
			else
			{
				ChangeSprite(Velocity);
			}
		}

		camera.Position = camera.Position.Lerp(cameraTarget, 0.2f);
	}

	void ChangeSprite(Vector2 vector)
	{
		vector = vector.Normalized();

		float deg = (360 - (Mathf.RadToDeg(Mathf.Atan2(vector.Y, vector.X)))) % 360;
		bool FlipH = false;

		if ((deg > 270 && deg <= 360) || (deg > 0 && deg <= 90))
		{
			//Flip H true
			FlipH = true;
		}
		else
		{
			//Flip H false
			FlipH = false;
		}

		Texture2D setText;

		//GD.Print(deg);

		if (deg > 15f && deg <= 75)
		{
			setText = Left3;
		}
		else if (deg > 75 && deg <= 105)
		{
			setText = Up;
		}
		else if (deg > 105 && deg <= 165)
		{
			setText = Left3;
		}
		else if (deg > 165 && deg <= 195)
		{
			setText = Left2;
		}
		else if (deg > 195 && deg <= 225)
		{
			setText = Left1;
		}
		else if (deg > 225 && deg <= 255)
		{
			setText = Left0;
		}
		else if (deg > 255 && deg <= 285)
		{
			setText = Down;
		}
		else if (deg > 285 && deg <= 315)
		{
			setText = Left0;
		}
		else if (deg > 315 && deg <= 345)
		{
			setText = Left1;
		}
		else
		{
			setText = Left2;
		}

		characterSprite.Texture = setText;
		characterSprite.FlipH = FlipH;
	}
}
