using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public Node HpSystem;
	public int MaxHealth {
		get => (int)HpSystem.Get("MaxHealth");
	}
	public int CurrentHealth {
		get => (int)HpSystem.Get("CurrentHealth");
	}
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	
	public override void _Ready() {
		HpSystem = GetNode<Node>("HealthComponent");
	}
	
	public override void _Process(double delta) {
		var label = GetNode<Label>("hpDev");
		label.Text = "HP: " + CurrentHealth;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if ((bool)HpSystem.Get("Dead")) {
			QueueFree();
		}
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		

		velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		Velocity = velocity;
		MoveAndSlide();
	}
}
