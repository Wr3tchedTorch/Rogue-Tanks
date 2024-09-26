using Godot;
using System;

public partial class GameManager : Node2D
{

	public override void _Process(double delta)
	{

		if (Input.IsActionPressed("restart"))
			GetTree().ReloadCurrentScene();
	}
}
