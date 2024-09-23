using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    [ExportGroup("Enemy Stats")]
    [Export] public float Health;
    [Export] public float Damage = 0.5f;
    [Export] public float Speed = 800;
    [Export] public float BulletSpeedBoost = 2;

    [ExportGroup("Bullet Stats")]
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 1200;
    [Export] public float FireRate = 100;

    public CharacterBody2D Target;

    public override void _Ready()
    {

        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {

        FollowPlayer();
    }

    private void FollowPlayer()
    {

        // Shoot backwards to gain impulse and pursue player.
        // Damages player by headbutting him.
        
    }
}
