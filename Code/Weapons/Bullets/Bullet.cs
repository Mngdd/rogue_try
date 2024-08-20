using Godot;
using System;

public partial class Bullet : RigidBody2D {
	public BulletTargets Target;
	public int Damage;
	private Vector2 Speed;
	// TODO: add additional damage, like dmg per sec for burn/toxic etc
	public enum BulletTargets {  // whos the target
		None, // set by default, used to raise errors
		Enemy,
		Player,
		Both  // for grenades idk
	}

	public override void _Ready() {
		Timer timer = GetNode<Timer>("Lifetime");
		timer.Timeout += () => QueueFree();
	}
	
	// FREEZE KINEMATIC SCENE PHYS + USE IT FOR STRAIGHT BULLETS
	public override void _IntegrateForces(PhysicsDirectBodyState2D state) {
		GlobalPosition += ConstantForce;
	}
	
	// todo: add global constexpr string PLAYERGROUP = "Player" and enemy too!
	private void OnBodyEntered(Node2D body) {
		bool wrongTargetEnemy = body.IsInGroup("Player") && Target == BulletTargets.Enemy;
		bool wrongTargetPlayer = body.IsInGroup("Enemy") && Target == BulletTargets.Player;
		bool notInGroup = !body.IsInGroup("Player") && !body.IsInGroup("Enemy");
		GD.Print("BULLET ENTER: " + body);

		if (wrongTargetEnemy || wrongTargetPlayer || notInGroup)
			return;  // cant hit him lol
		body.GetNode<Node>("HealthComponent").Call("Damage", Damage);
		GD.Print("HIT!");
	}
}
