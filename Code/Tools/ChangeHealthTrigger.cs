using Godot;
using System;

public partial class ChangeHealthTrigger : Node2D {
	private bool _canDamage = false;
	[Export] public int HealthAdd = 10;
	[Export] public bool PushBack = false;  //TODO: IMPLEMENT ME
	[Export] public double ActInterval = 1d;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
	//TODO: FIX ENTER-ACT-TIMOUT-EXIT DAMAGE SEQUENCE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	private void _on_area_2d_body_entered(Node2D body) {
		// Replace with function body.
		//if (body is CharacterBody2D) // можно и другие сравнения...
		if (body is Player) {
			_canDamage = true;
			GD.Print("ENTITY ENTERED: " + body);
			
			var timer = GetNode<Timer>("Cooldown");
			timer.Connect("timeout", new Callable(this, MethodName._act));
		}
	}

	private void _act(Node2D body) {
		Player pl = (Player)body;
		pl.AddHealth(HealthAdd);
	}
	
	private void _on_area_2d_body_exited(Node2D body) {
		if (body is Player) {
			_canDamage = false;
			GD.Print("ENTITY EXITED: " + body);
			GetNode<Timer>("Cooldown").Stop();
			GetNode<Timer>("Cooldown").WaitTime = ActInterval;
		}
		// Replace with function body.
	}

	private void _on_cooldown_timeout() {
		if (_canDamage) {
			GD.Print("DOIN_DAMAGE");
			GetNode<Timer>("Cooldown").Start();
		} else {
			GD.Print("exited trigga");
		}
	}
}
