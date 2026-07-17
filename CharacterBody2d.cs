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

	public Vector2 cameraTarget;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

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

		camera.Position = camera.Position.Lerp(cameraTarget, 0.2f);
	}
}
