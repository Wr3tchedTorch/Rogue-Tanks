using Godot;
using System;
using System.Linq;

public partial class DroneComponent : Node2D
{

	[Export(PropertyHint.Range, "0, 100")] public float FireRate = 60;
	[Export] public PackedScene DroneScene;

	private Timer _fireRateTimer;
	private Tank _parent;
	private Node _bulletsGroup;

	public override void _Ready()
	{

		_fireRateTimer = GetNode<Timer>("FireRateTimer");
		_bulletsGroup = GetTree().GetFirstNodeInGroup("Bullets");
		_parent = GetParent<Tank>();		

		float droneTimerDelay = 2 - (1.5f * FireRate / 100);
		_fireRateTimer.WaitTime = droneTimerDelay;
		_fireRateTimer.Timeout += OnFireRateTimerTimeout;
		_fireRateTimer.Start();
	}

	public override void _Process(double delta)
	{
	}

	private void OnFireRateTimerTimeout()
	{

		LaunchDrone();
	}

	private void LaunchDrone()
	{

		Drone droneInstance = DroneScene.Instantiate<Drone>();
		_bulletsGroup.AddChild(droneInstance);
		droneInstance.GlobalPosition = GlobalPosition;
		droneInstance.Target = GetClosestEnemy();
		
		var randomNum = new Random().Next(0, 100);
		var bulletDamage = _parent.CriticalChance > randomNum ? _parent.Damage + (_parent.Damage * .5f) : _parent.Damage;
		droneInstance.Damage = bulletDamage;
	}

	private CharacterBody2D GetClosestEnemy()
	{

		Node[] nodes = GetTree().GetFirstNodeInGroup("Enemies").GetChildren().ToArray();
		GD.Print(nodes.Length);
		CharacterBody2D[] enemies = nodes.OfType<CharacterBody2D>().ToArray();

		GD.Print(enemies.Length);

		if (enemies.Length == 0)
			return null;

		CharacterBody2D closestEnemy = enemies[0];
		Vector2 closestEnemyDirection = enemies[0].GlobalPosition - GlobalPosition;
		foreach (CharacterBody2D enemy in enemies)
		{
			Vector2 thisEnemyDirection = enemy.GlobalPosition - GlobalPosition;

			if (thisEnemyDirection.Length() < closestEnemyDirection.Length())
			{
				closestEnemyDirection = thisEnemyDirection;
				closestEnemy = enemy;
			}
		}
		return closestEnemy;
	}
}
