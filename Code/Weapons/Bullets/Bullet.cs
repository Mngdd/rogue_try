using Godot;
using System;

public partial class Bullet : RigidBody2D
{
	public enum BulletTargets {  // кого дамажим
		Enemy,
		Player,
		Both  // гранаты типа
	}

	public override void _Ready() {
		Timer timer = GetNode<Timer>("Lifetime");
		timer.Timeout += () => QueueFree();
	}

	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body) {
		// if (bodt
	}
}
