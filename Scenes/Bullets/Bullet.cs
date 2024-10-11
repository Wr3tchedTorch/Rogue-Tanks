using Godot;
using System;

public partial class Bullet : CharacterBody2D
{

	[ExportGroup("Physics Attributes")]
	[Export(PropertyHint.Range, "0, 2000")] public float MaxSpeed = 1600;
	[Export(PropertyHint.Range, "0, 100")] public float MaxSteering = 100;
	[Export(PropertyHint.Range, "0, 2")] public float Mass = 1;

	public float Speed
	{
		get => _speed;
		set => _speed = value >= 0 ? value : throw new ArgumentException("Speed can't be lower than `0`.");
	}
	public float Damage
	{
		get => _damage;
		set => _damage = value >= 0 ? value : throw new ArgumentException("Damage can't be lower than `0`.");
	}

	public Vector2 Direction;

	private float _damage;
	private float _speed;

	public override void _PhysicsProcess(double delta)
	{
		
		Vector2 steering = GetForce();
		steering = steering.LimitLength(MaxSteering);
		steering /= Mass;

		Velocity = (Velocity + steering).LimitLength(MaxSpeed);
		MoveAndSlide();
	}

	public virtual Vector2 GetForce()
	{
		return Direction.Normalized() * Speed;
	}

	public void InitializeAttributes(float damage, float speed, Vector2 direction)
	{
		Speed = speed;
		Damage = damage;
		Direction = direction;
	}

	public void OnVisibleOnScreenNotifier2DScreenExited()
		=> QueueFree();
}
