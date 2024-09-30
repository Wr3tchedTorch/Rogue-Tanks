using Godot;
using System;

public partial class GameManager : Node2D
{

	private MobSpawner _mobSpawner;	
	private double _enemySpawnDelay;
	private double _startTime;

	public override void _Ready()
	{

		_startTime = Time.GetUnixTimeFromSystem();
		_mobSpawner = GetTree().GetFirstNodeInGroup("MobSpawner") as MobSpawner;
	}

	public override void _Process(double delta)
	{

		SpawnEnemies();

		if (Input.IsActionPressed("restart"))
			GetTree().ReloadCurrentScene();
	}

	private void SpawnEnemies()
	{

		double _endTime = Time.GetUnixTimeFromSystem();
		double elapsedTime = _endTime - _startTime;
		
		_enemySpawnDelay = 5.0d - (4.9d * (elapsedTime / 300d));
		_mobSpawner.SpawnDelay = (float)_enemySpawnDelay;
	}

}
