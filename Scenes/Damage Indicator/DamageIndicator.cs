using Godot;
using System;

public partial class DamageIndicator : Node2D
{

	[ExportGroup("Label")]
	[Export] public float LabelValue { get; set; }

	[ExportGroup("Physics")]
	[Export] public float Speed = 400;
	[Export] public float Friction = 200;

	private Vector2 _shiftDirection;

	public override void _Ready()
	{

		_shiftDirection = Vector2.Up;

		GetNode<Label>("%DamageLabel").Text = String.Format("{0:0.00}", LabelValue);
	}

	public override void _PhysicsProcess(double delta)
	{

		GlobalPosition += Speed * _shiftDirection * (float)delta;
		Speed = Mathf.Max(Speed - Friction * (float)delta, 0);
	}
}
