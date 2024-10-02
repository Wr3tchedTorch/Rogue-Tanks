using Godot;
using System;

public partial class GameManager : Node2D
{

	private ProgressBar _experienceBar;
	private Label _playerLevel;
	private Tank _player;
	private MobSpawner _mobSpawner;
	private Node2D _experienceGroup;

	private double _enemySpawnDelay;
	private double _startTime;

	public override void _Ready()
	{

		_startTime = Time.GetUnixTimeFromSystem();
		_mobSpawner = GetTree().GetFirstNodeInGroup("MobSpawner") as MobSpawner;

		_experienceBar = GetNode<ProgressBar>("%ExperienceBar");
		_playerLevel = GetNode<Label>("%PlayerLevel");

		_player = GetTree().GetFirstNodeInGroup("Player") as Tank;
		_player.LevelUp += OnPlayerLevelUp;

		_experienceGroup = GetTree().GetFirstNodeInGroup("Experience") as Node2D;
	}

	public override void _Process(double delta)
	{

		_experienceBar.Value = _player.Exp;

		SpawnEnemies();

		if (Input.IsActionPressed("restart"))
			GetTree().ReloadCurrentScene();
	}

	public void OnPlayerLevelUp()
	{

		_playerLevel.Text = $"PLAYER LEVEL: {_player.CurrentLevel}";
		_experienceBar.MaxValue = _player.ExpRequiredForLevelUp;
	}

	private void SpawnEnemies()
	{

		double _endTime = Time.GetUnixTimeFromSystem();
		double elapsedTime = _endTime - _startTime;

		_enemySpawnDelay = 1.0d - (.9d * (elapsedTime / 300d));
		_mobSpawner.SpawnDelay = (float)_enemySpawnDelay;
	}

}
