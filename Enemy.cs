using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    [ExportGroup("Enemy Stats")]
    [Export]
    public float Armor
    {
        get => _armor;
        set => _armor = value is >= 0 and <= 100 ? value : throw new ArgumentException($"{nameof(Armor)} must be at least 0 and at most 100.");
    }
    [Export] public float Damage = 0.5f;
    [Export] public float Speed = 130;
    [Export] public float MaxSpeed = 1200;
    [Export] public float BulletForce = 600;
    [Export] public float Mass = 1.5f;

    [ExportGroup("Bullet Stats")]
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 1200;
    [Export] public float FireRate = 100;

    public CharacterBody2D Target;

    protected bool _freeze = false;
	private float _armor = 50;

    public override void _Ready()
    {

        Target = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;

        GetNode<HealthComponent>("HealthComponent").Armor = Armor;
    }

    public void SetPause(bool pause) => _freeze = pause;

    protected virtual void FollowTarget()
    {
    }

    protected Vector2 GetFollowForce()
    {
        Vector2 desiredVelocity = (Target.GlobalPosition - GlobalPosition).Normalized() * Speed;
        return desiredVelocity - Velocity;
    }
}
