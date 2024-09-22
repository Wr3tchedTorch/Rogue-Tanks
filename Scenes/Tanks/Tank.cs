using System;
using Godot;
using Godot.Collections;

public partial class Tank : CharacterBody2D
{

	[ExportGroup("Gameplay Attributes")]
	[Export] public float Damage { get; set; }
	[Export] public float BulletSpeed { get; set; } = 1200;
	[Export] public float FireRate { get; set; } = 1000;
	[Export] public PackedScene BulletScene;

	[ExportGroup("Physics Attributes")]
	[Export] public float Mass { get; set; } = 1.2f;
	[Export] public int Speed { get; set; } = 100;

	private Sprite2D _spr;
	private Timer _shotDelayTimer;
	private float Accel = 5.0f;
	private bool _canShot = true;
	private Vector2 _targetPos;

	public override void _Ready()
	{

		_spr = GetNode<Sprite2D>("Sprite2D");
		_shotDelayTimer = GetNode<Timer>("Gun/ShotDelay");
		_shotDelayTimer.Timeout += OnShootDelayTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{

		_targetPos = GetGlobalMousePosition();

		HandleMovement(delta);

		if (_canShot && Input.IsActionPressed("shoot"))
			HandleShooting();
	}

	public void OnHealthComponentDeath()
	{

		QueueFree();
	}

	public void OnShootDelayTimeout()
		=> _canShot = true;

	private void HandleMovement(double delta)
	{

		LookAt(_targetPos);

		Vector2 dir = Input.GetVector("left", "right", "up", "down").Normalized();
		Velocity = Velocity.Lerp(dir * Speed, (float)delta * Accel / Mass);
		MoveAndSlide();
	}

	private void HandleShooting()
	{

		_canShot = false;
		_shotDelayTimer.WaitTime = 100 / FireRate;
		_shotDelayTimer.Start();

		GetNode<Node2D>("Gun/ShotingSpawners").LookAt(_targetPos);
		Array<Node> spawnersList = GetNode<Node2D>("Gun/ShotingSpawners").GetChildren();

		foreach (Node2D node in spawnersList)
		{

			Array<Node> _group = node.GetChildren();

			Marker2D spawner = (Marker2D)_group[(int)Mathf.Ceil(new Random().NextDouble() * _group.Count - 1)];

			Bullet bullet = BulletScene.Instantiate<Bullet>();
			GetTree().GetFirstNodeInGroup("Bullets").AddChild(bullet);
			bullet.GlobalPosition = spawner.GlobalPosition;
			bullet.InitializeAttributes(Damage, BulletSpeed, Vector2.Right.Rotated(Rotation));
		}

	}
}
