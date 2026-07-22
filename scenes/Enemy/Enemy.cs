using System;
using System.Collections.Generic;
using Godot;

public struct MovementHeft
{
	public enum TargetType
	{
		Player,
		Horde,
		AntiClump,
	}

	public TargetType targetType;
	public Vector2 direction;
	public float weight;
}

public partial class Enemy : CharacterBody2D
{
	[Export]
	Area2D Hitbox;

	[Export]
	Area2D Hurtbox;

	[Export]
	Area2D Visionbox;

	public const float Speed = 80.0f;

	bool aggro;

	//[Export]
	public List<MovementHeft> Heft;

	[Export]
	public Player player;

	//public const float JumpVelocity = -400.0f;
	private float m_Speed;
	private Vector2 MovementVector;

	public override void _PhysicsProcess(double delta)
	{
		foreach (Node2D body in Visionbox.GetOverlappingBodies())
		{
			if (body is Player)
			{
				aggro = true;
				player = (Player)body;

				if (Heft == null)
				{
					Heft = new List<MovementHeft>();

					MovementHeft testHeft = new MovementHeft();

					testHeft.targetType = MovementHeft.TargetType.Player;
					testHeft.weight = 1;
					testHeft.direction = (player.GlobalPosition - this.GlobalPosition).Normalized();

					Heft.Add(testHeft);
				}
				else
				{
					MovementHeft testHeft2 = Heft[0];

					testHeft2.direction = (player.GlobalPosition - this.GlobalPosition).Normalized();

					Heft[0] = testHeft2;
				}
			}
		}

		if (player != null)
		{
			GD.Print("I See the player");
		}

		Vector2 velocity = Velocity;

		Vector2 direction = Vector2.Zero;

		if (Heft != null)
		{
			direction = Heft[0].direction;
		}

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
