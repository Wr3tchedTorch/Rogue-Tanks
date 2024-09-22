using Godot;
using System;

public partial class Bullet : CharacterBody2D
{

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

		Velocity = Direction.Normalized() * Speed;
		MoveAndSlide();
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
