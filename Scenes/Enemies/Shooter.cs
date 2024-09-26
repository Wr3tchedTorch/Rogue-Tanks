using Godot;
using Godot.Collections;
using System;

public partial class Shooter : Enemy
{

    private Timer _shootTimer;
    private Timer _dashDelayTimer;
    private bool _canShot = true;
    private bool _isDashing = false;

    public override void _Ready()
    {

        base._Ready();

        _shootTimer = GetNode<Timer>("ShootingDelay");
        _shootTimer.Timeout += OnShootDelayTimeout;

        _dashDelayTimer = GetNode<Timer>("DashDelay");
        _dashDelayTimer.Timeout += OnDashTimerTimeout;

        GetNode<Area2D>("HurtBox").BodyEntered += OnHurtBoxBodyEntered;

        float fireDelay = 10 / FireRate;
        _shootTimer.WaitTime = fireDelay;
    }

    public override void _PhysicsProcess(double delta)
    {

        if (_freeze) return;

        FollowTarget();

        Vector2 dir = (Target.GlobalPosition - GlobalPosition).Normalized();
        Vector2 oppositeSide = GlobalPosition - GlobalPosition.DistanceTo(Target.GlobalPosition) * dir;

        PointAwayFromTarget(oppositeSide);

        if (!_canShot)
            return;
        _canShot = false;
        _shootTimer.Start();

        SpawnBullet(oppositeSide, -dir);
    }

    public void OnShootDelayTimeout() => _canShot = true;

    protected void PointAwayFromTarget(Vector2 oppositeSide)
        => GetNode<Sprite2D>("Sprite2D").LookAt(oppositeSide);

    protected override void FollowTarget()
    {
        Vector2 steering = Vector2.Zero;
        steering += GetFollowForce() + GetBulletForce();
        steering = steering.LimitLength(120);
        steering /= Mass;

        Velocity = (Velocity + steering).LimitLength(MaxSpeed);
        MoveAndSlide();
    }

    private Marker2D GetRandomSpawner(Node spawnerGroup)
    {
        Array<Node> group = spawnerGroup.GetChildren();
        int randomIndex = (int)Mathf.Ceil(new Random().NextDouble() * group.Count - 1);
        return group[randomIndex] as Marker2D;
    }

    private void SpawnBullet(Vector2 spawnDirection, Vector2 bulletDirection)
    {

        _isDashing = true;
        var gun = GetNode<Node2D>("Gun/ShotingSpawners");
        gun.LookAt(spawnDirection);

        Array<Node> spawnersList = gun.GetChildren();
        Node bulletsGroup = GetTree().GetFirstNodeInGroup("Bullets");

        foreach (Node2D node in spawnersList)
        {

            Marker2D spawner = GetRandomSpawner(node);

            Bullet bullet = BulletScene.Instantiate<Bullet>();
            bulletsGroup.AddChild(bullet);
            bullet.GlobalPosition = spawner.GlobalPosition;
            bullet.InitializeAttributes(Damage, BulletSpeed, bulletDirection);
        }
    }

    private Vector2 GetBulletForce()
    {

        if (!_isDashing)
            return Vector2.Zero;

        if (_dashDelayTimer.IsStopped()) _dashDelayTimer.Start();
        Vector2 dir = (Target.GlobalPosition - GlobalPosition).Normalized();

        var bulletForce = dir * 600;
        return bulletForce;
    }

    public void OnDashTimerTimeout()
        => _isDashing = false;

    public void OnHurtBoxBodyEntered(Node2D area)
    {

        if (area is Bullet bullet)
        {
            GetNode<HealthComponent>("HealthComponent").DealDamage(bullet.Damage);
            bullet.QueueFree();
        }
    }

    public void OnHealthComponentDeath()
    {

        // Death Animation
        QueueFree();
    }
}
