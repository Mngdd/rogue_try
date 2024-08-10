using Godot;
using System;

public partial class ChangeHealthTrigger : Node2D {
	
	private bool _canDamage = false;
	[Export] public int HealthAdd = 0;
	[Export] public bool PushBack = false;  //TODO: IMPLEMENT ME
	[Export] public double ActInterval = 2d;
	public override void _Ready() {
	}

	public override void _Process(double delta) {
	}
	
	// костыльно, таймеры (возможно они вредны но я хз)
	private void _on_area_2d_body_entered(Node2D body) {
		//if (body is CharacterBody2D) // можно и другие сравнения...
		if (body is Player) {
			_canDamage = true;
			GD.Print("ENTITY ENTERED: " + body);

			Callable callable = Callable.From(() => _act(body));
			// FIXME: при повторном входе создается дупликат таймера, нужно пофиксить!!

			var timer = GetNode<Timer>("Cooldown");
			timer.Connect("timeout", callable);
			timer.WaitTime = ActInterval;
			timer.Start();
		}
	}
	
	private void _on_area_2d_body_exited(Node2D body) {
		if (body is Player) {
			_canDamage = false;
			GD.Print("ENTITY EXITED: " + body);
			
			var timer = GetNode<Timer>("Cooldown");
			timer.Stop();
			timer.WaitTime = ActInterval;
		}
	}
	
	private void _act(Node2D body) {
		if (!_canDamage)
			return;
		
		GD.Print("_act, does damage stuff lolll");
		Player pl = (Player)body;
		pl.HpSystem.Call("AddHealth", HealthAdd);
		// then repeat...
	}
	
	// obsolete rn
	private void _on_cooldown_timeout() {
		// if (_canDamage) {
		// 	EmitSignal(SignalName.InteractWithEntity);
		// } else {
		// 	GD.Print("exited trigga");
		// }
	}
}
