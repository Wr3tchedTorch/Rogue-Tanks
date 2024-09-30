using Godot;
using System;

public partial class MobSpawner : Path2D
{

	[Export] public PackedScene[] enemies;
	[Export]
	public float SpawnDelay
	{
		get => _spawnDelay;
		set => _spawnDelay = value >= 0 ? value : throw new ArgumentException($"{nameof(SpawnDelay)} must be higher or equal to 0.");
	}

	private Node2D _enemiesParent;
	private Timer _spawnDelayTimer;
	private float _spawnDelay = 5f;
	private readonly Random _RNG = new();

	/* This will be used to pick a random position 
	   outside the screen */
	private PathFollow2D _pathFollow;

	public override void _Ready()
	{

		_spawnDelayTimer = GetNode<Timer>("Timer");
		_spawnDelayTimer.WaitTime = SpawnDelay;

		_pathFollow = GetNode<PathFollow2D>("PathFollow2D");
		_enemiesParent = GetTree().GetFirstNodeInGroup("Enemies") as Node2D;
	}

	public override void _Process(double delta)
	{

		if (!_spawnDelayTimer.IsStopped())
			return;

		_spawnDelayTimer.WaitTime = SpawnDelay;
		SpawnRandomEnemy();
		_spawnDelayTimer.Start();
	}

	public void SpawnRandomEnemy()
	{

		double spawnChance = _RNG.NextDouble();
		GD.Print($"spawnChance: {spawnChance}");
		foreach (PackedScene enemyScene in enemies)
		{

			Enemy enemy = enemyScene.Instantiate<Enemy>();
			if (enemy.SpawnProbability < spawnChance)
				continue;

			_enemiesParent.AddChild(enemy);
			enemy.GlobalPosition = GetRandomPos();
		}
	}

	public void SpawnEnemy(PackedScene enemyScene)
	{

		Enemy enemy = enemyScene.Instantiate<Enemy>();

		_enemiesParent.AddChild(enemy);
		enemy.GlobalPosition = GetRandomPos();
	}

	private Vector2 GetRandomPos()
	{
		_pathFollow.ProgressRatio = (float)_RNG.NextDouble();
		return _pathFollow.GlobalPosition;
	}

}
