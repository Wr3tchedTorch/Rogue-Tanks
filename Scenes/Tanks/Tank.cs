using System;
using Godot;
using Godot.Collections;

public partial class Tank : CharacterBody2D
{

	[ExportGroup("Gameplay Attributes")]
	[Export]
	public float Armor
	{
		get => _armor;
		set => _armor = value is >= 0 and <= 100 ? value : throw new ArgumentException($"{nameof(Armor)} must be at least 0 and at most 100.");
	}
	[Export] public float Damage { get; set; } = 10;
	[Export] public float CriticalChance { get; set; } = 15;
	[Export] public float BulletSpeed { get; set; } = 1200;
	[Export] public float FireRate { get; set; } = 15;
	[Export] public PackedScene BulletScene;

	[ExportGroup("Physics Attributes")]
	[Export] public float Mass { get; set; } = 1.2f;
	[Export] public int Speed { get; set; } = 700;

	private Sprite2D _spr;
	private Timer _shotDelayTimer;
	private HealthComponent _healthComponent;

	private float _armor = 50;
	private float Accel = 5.0f;
	private Vector2 _targetPos;

	private bool _canShot = true;
	private bool _isHurtBoxColliding = false;
	private Node2D _hurtBoxCollidingBody;

	public override void _Ready()
	{

		_spr = GetNode<Sprite2D>("Sprite2D");

		_shotDelayTimer = GetNode<Timer>("Gun/ShotDelay");
		_shotDelayTimer.Timeout += OnShootDelayTimeout;

		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthComponent.Armor = Armor;

		GetNode<Area2D>("HurtBox").BodyEntered += OnHurtBoxBodyEntered;
		GetNode<Area2D>("HurtBox").BodyExited += OnHurtBoxBodyExited;
	}

	public override void _Process(double delta)
	{

		if (_isHurtBoxColliding)
			TakeDamage();
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

		// Death Animation
		QueueFree();
	}

	public void OnShootDelayTimeout()
		=> _canShot = true;

	public void OnHurtBoxBodyExited(Node2D body)
		=> _isHurtBoxColliding = false;

	public void OnHurtBoxBodyEntered(Node2D body)
	{
		_isHurtBoxColliding = true;
		_hurtBoxCollidingBody = body;
	}

	private void HandleMovement(double delta)
	{

		GetNode<Sprite2D>("Sprite2D").LookAt(_targetPos);

		Vector2 dir = Input.GetVector("left", "right", "up", "down").Normalized();
		Velocity = Velocity.Lerp(dir * Speed, (float)delta * Accel / Mass);
		MoveAndSlide();
	}

	private void HandleShooting()
	{

		GetNode<AnimationPlayer>("AnimationPlayer").Play("Tanks/Shoot");
		_canShot = false;
		_shotDelayTimer.WaitTime = 10 / FireRate;
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

			var randomNum = new Random().Next(0, 100);
			var bulletDamage = CriticalChance > randomNum ? Damage + (Damage * .5f) : Damage;
			bullet.InitializeAttributes(bulletDamage, BulletSpeed, Vector2.Right.Rotated(GetNode<Sprite2D>("Sprite2D").Rotation));
		}
	}

	private void TakeDamage()
	{

		if (_hurtBoxCollidingBody is Bullet bullet)
		{
			bullet.QueueFree();
			_healthComponent.DealDamage(bullet.Damage);
		}
		if (_hurtBoxCollidingBody is Enemy enemy)
			_healthComponent.DealDamage(enemy.Damage);
	}
}
