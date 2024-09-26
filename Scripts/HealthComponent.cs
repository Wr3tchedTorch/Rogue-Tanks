using Godot;
using System;

public partial class HealthComponent : Node2D
{

	[ExportGroup("Stats")]
	[Export]
	public float Armor
	{
		get => _armor;
		set => _armor = value is >= 0 and <= 100 ? value : throw new ArgumentException($"{nameof(Armor)} must be at least 0 and at most 100.");
	}
	[Export] public float InvencibilityDuration = .5f;

	[Signal] public delegate void DeathEventHandler();

	public float Health = 100;

	private float _armor = 50;
	private Timer _invencibilityTimer;

	public override void _Ready()
	{

		_invencibilityTimer = GetNodeOrNull<Timer>("InvencibilityTimer");
		if (_invencibilityTimer != null)
			_invencibilityTimer.WaitTime = InvencibilityDuration;
	}

	public override void _Process(double delta)
	{

		ProgressBar bar = GetNodeOrNull<ProgressBar>("HealthBar");
		if (bar == null)
			return;
		bar.Value = Health;
	}

	public void DealDamage(float damage)
	{
		if (damage == 0)
			return;

		if (IsInvencibilityTimerPlaying())
			return;					

		damage /= GetArmorMultiplier();
		Health = Mathf.Max(0, Health - damage);

		GD.Print($"{nameof(damage)}: {damage}");
		GD.Print($"{nameof(Health)}: {Health}");

		if (Health == 0)
			EmitSignal(SignalName.Death);
	}

	public void Heal(int value)
		=> Health = Mathf.Min(100, Health + value);

	private float GetArmorMultiplier()
	{

		float percentage = Armor / 100;
		return 2 * percentage;
	}

	private bool IsInvencibilityTimerPlaying()
	{

		if (_invencibilityTimer == null)
			return false;

		if (!_invencibilityTimer.IsStopped())
			return true;
				
		_invencibilityTimer.Start();
		return false;
	}
}
