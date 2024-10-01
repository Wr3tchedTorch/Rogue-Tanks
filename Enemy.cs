using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    [ExportGroup("Enemy Stats")]
    [Export(PropertyHint.Range, "0,100")] public float ExperienceGain = 15;
    [Export(PropertyHint.Range, "0,1,")]
    public float SpawnProbability
    {
        get => _spawnProbability;
        set => _spawnProbability = value is >= 0 and <= 1 ? value : throw new ArgumentOutOfRangeException($"{nameof(SpawnProbability)} must be higher than 0 and lower than 1.");
    }
    [Export(PropertyHint.Range, "0,100,")]
    public float Armor
    {
        get => _armor;
        set => _armor = value is >= 0 and <= 100 ? value : throw new ArgumentException($"{nameof(Armor)} must be at least 0 and at most 100.");
    }
    [Export] public float Damage = 0.5f;
    [Export(PropertyHint.Range, "0,100,")] public float FireRate = 100;

    [ExportGroup("Physics Stats")]
    [Export] public float Speed = 130;
    [Export] public float MaxSpeed = 1200;
    [Export] public float BulletForce = 600;
    [Export] public float Mass = 1.5f;

    [ExportGroup("Bullet Stats")]
    [Export] public PackedScene BulletScene;
    [Export] public float BulletSpeed = 1200;

    public CharacterBody2D Target;

    protected bool _freeze = false;

    private float _armor = 50;
    private float _spawnProbability = 0.5f;

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

    public void OnHealthComponentDeath()
    {

        SpawnExpOrb();

        GetNode<AnimationPlayer>("AnimationPlayer").Play("Tanks/Death");
    }

    private void SpawnExpOrb()
    {
        
        PackedScene expScene = GD.Load<PackedScene>("res://Scenes/Experience Orbs/experience_orb.tscn");

        ExperienceOrb exp = expScene.Instantiate<ExperienceOrb>();
        GetTree().GetFirstNodeInGroup("Experience").CallDeferred("add_child", exp);
        exp.ExperienceGain = ExperienceGain;
        exp.GlobalPosition = GlobalPosition;
    }
}
