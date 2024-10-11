using Godot;

public partial class Drone : Bullet
{

	public CharacterBody2D Target;

	public override void _Ready()
	{
		Speed = 1000;
	}

	public override Vector2 GetForce()
	{
		if (!IsInstanceValid(Target) || Target.IsQueuedForDeletion())
		{
			QueueFree();
			return Vector2.Zero;
		}

		return (Target.GlobalPosition - GlobalPosition).Normalized() * Speed;
	}
}
