using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
	public List<Enemy> Allies;

	[Export]
	public Player player;

	//public const float JumpVelocity = -400.0f;
	public float m_maxSpeed;
	private float m_Speed;
	private Vector2 MovementVector;
	public int m_size;
	public float m_MaxHealth;
	public float m_Health;

	public override void _Ready()
	{
		base._Ready();
		m_size = 5;
		m_Speed = 75.0f;
		//m_Speed = m_maxSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Heft = new List<MovementHeft>();
		Allies = new List<Enemy>();

		foreach (Node2D body in Visionbox.GetOverlappingBodies())
		{
			if (!aggro)
			{
				if (body is Player)
				{
					aggro = true;
					player = (Player)body;
				}
			}

			if (player != null)
			{
				MovementHeft testHeft = new MovementHeft();

				testHeft.targetType = MovementHeft.TargetType.Player;
				testHeft.weight = 2f;
				testHeft.direction = (player.GlobalPosition - this.GlobalPosition).Normalized();

				Heft.Add(testHeft);
			}

			if (body is Enemy)
			{
				Allies.Add((Enemy)body);

				//check if too close

				float XDiff = GlobalPosition.X - body.GlobalPosition.X;
				float YDiff = GlobalPosition.Y - body.GlobalPosition.Y;

				float distanceApart = MathF.Sqrt((XDiff * XDiff) + (YDiff * YDiff));

				if (distanceApart <= m_size + ((Enemy)body).m_size)
				{
					//too close
					MovementHeft tooCloseHeft = new MovementHeft();

					tooCloseHeft.targetType = MovementHeft.TargetType.AntiClump;
					tooCloseHeft.weight = 4.0f;
					tooCloseHeft.direction = (this.GlobalPosition - body.GlobalPosition).Normalized();

					Heft.Add(tooCloseHeft);

					//GD.Print("Too Close");
				}
			}
		}

		if (Allies.Count > 0)
		{
			Vector2 totalPosition = Vector2.Zero;

			for (int i = 0; i < Allies.Count; i++)
			{
				if (aggro)
				{
					if (!Allies[i].aggro)
					{
						Allies[i].aggro = true;
						Allies[i].player = player;
					}
				}

				totalPosition += Allies[i].GlobalPosition;
			}
			Vector2 averagePosition = totalPosition / Allies.Count;

			MovementHeft hordeHeft = new MovementHeft();

			hordeHeft.targetType = MovementHeft.TargetType.Horde;
			hordeHeft.weight = 0.5f;
			hordeHeft.direction = (averagePosition - this.GlobalPosition).Normalized();

			float XDiff2 = averagePosition.X - GlobalPosition.X;
			float YDiff2 = averagePosition.Y - GlobalPosition.Y;

			float distanceApart2 = MathF.Sqrt((XDiff2 * XDiff2) + (YDiff2 * YDiff2));

			if (distanceApart2 < m_size * 2 || aggro)
			{
				hordeHeft.weight = -0.0f;
			}

			Heft.Add(hordeHeft);
		}

		Vector2 velocity = Velocity;

		Vector2 direction = Vector2.Zero;

		if (Heft.Count != 0)
		{
			foreach (MovementHeft mh in Heft)
			{
				direction += mh.direction * mh.weight;
			}
			if (direction.Length() >= 1.0f)
			{
				direction = direction.Normalized();
			}
		}
		else
		{
			direction = Vector2.Zero;
		}

		if (direction != Vector2.Zero)
		{
			//m_Speed = m_maxSpeed;

			velocity = direction * m_Speed;
		}
		else
		{
			//m_Speed = Mathf.Lerp(m_Speed, 0, 0.8f);
			velocity = Vector2.Zero;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
