using Godot;
using System;

public partial class ExperienceOrb : CharacterBody2D
{

	[Export] public float ExperienceGain = 15;
	[Export] public float DistanceRadius = 600;

	[ExportGroup("Physics")]
	[Export] public float MaxVelocity = 800;
	[Export] public float MaxSteering = 100;

	private float _mass = 1;
	private Tank _player;

	public override void _Ready()
	{

		Scale = Vector2.One * (float)Math.Max(ExperienceGain / 50, .8);
		GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
		_mass = Scale.X;

		_player = GetTree().GetFirstNodeInGroup("Player") as Tank;
	}

    public override void _PhysicsProcess(double delta)
    {

		FollowPlayer();
    }

    public void OnBodyEntered(Node2D body)
	{
		if (body is Tank tank)
		{
			tank.AddExperience(ExperienceGain);
			QueueFree();
		}
	}

	private void FollowPlayer()
	{

		Vector2 desiredVelocity = _player.GlobalPosition - GlobalPosition;
		if (desiredVelocity.Length() > DistanceRadius)
			return;

		Vector2 steering = desiredVelocity;
		steering = steering.LimitLength(MaxSteering);
		steering /= _mass;

		Velocity = (Velocity + steering).LimitLength(MaxVelocity);
		MoveAndSlide();
	}
}

