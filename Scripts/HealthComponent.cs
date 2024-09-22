using Godot;
using System;

public partial class HealthComponent : Node2D
{

	[Export]
	public float Health
	{
		get => _health;
		set => _health = value is >= 0 and <= 100 ? value : throw new ArgumentException("Health can't be lower than `0` or higher than a `100`");
	}

	[Signal] public delegate void DeathEventHandler();

	private float _health;
	private Area2D _hurtBox;

	public override void _Ready()
	{

		_hurtBox = GetNode<Area2D>("HurtBox");
		_hurtBox.BodyEntered += OnHurtBoxBodyEntered;

		Death += GetParent<Tank>().OnHealthComponentDeath;
		Health = 100;
	}

	public override void _Process(double delta)
	{

		(GetTree().GetFirstNodeInGroup("PlayerHealthBar") as ProgressBar).Value = Health;
	}

	public void GetDamage(float damage)
	{
		Health = Mathf.Max(0, Health - damage);

		if (Health == 0)
			EmitSignal(SignalName.Death);
	}

	public void Heal(int value)
		=> Health = Mathf.Min(100, Health + value);

	public void OnHurtBoxBodyEntered(Node2D body) {
		if (body is Bullet bullet)
			GetDamage(bullet.Damage);
		// else if (body is Enemy enemy)
			// GetDamage(enemy.Damage);
	}
}
