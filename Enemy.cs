using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    [ExportGroup("Enemy Stats")]
    [Export] public float Health;
    [Export] public float Damage = 0.5f;
    [Export] public float Speed = 130;
    [Export] public float MaxSpeed = 1200;
    [Export] public float BulletSpeedBoost = 3;
    [Export] public float Mass = 1.5f;

    [ExportGroup("Bullet Stats")]
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 1200;
    [Export] public float FireRate = 100;

    public CharacterBody2D Target;

    protected bool _freeze = false;

    public override void _Ready()
    {

        Target = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
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
