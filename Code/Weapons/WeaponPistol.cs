namespace rogue_try.Code.Weapons;

using Godot;
using System;

public partial class WeaponPistol : WeaponFirearm {
	// public override void _Ready() {
	// 	base._Ready();
	// 	GD.Print("pistol init");
	// 	//BulletScene = ResourceLoader.Load<PackedScene>("res:// scene. tscn").Instantiate();
	// 	
	//
	// }

	public override void Fire() { // TODO: CUT IT INTO FIREARM BASE!!!!!!!!
		base.Fire(); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		
		if (!_canShoot()) {
			return;
		}

		if (AmmoCurrent == 0) {
			CanShootReload = false;
			Reaload();
			return;
		}

		--AmmoCurrent;
		//CanShoot = false;

		CooldownTimer.Start();
		CanShootCooldown = false;
		// TODO: sound?
		// todo move to parent + put into another function
		//fixme: bullet collision!
		GD.Print("pew, ammo: " + AmmoCurrent + "/" + AmmoMax);

		RigidBody2D bullet = BulletScene.Instantiate<RigidBody2D>();
		bullet.TopLevel = true;
		bullet.Rotation = GetAngleTo(GetGlobalMousePosition());
		bullet.GlobalPosition = GetNode<Node2D>("MuzzleSlot").GlobalPosition;
		
		// normalize vector velocity ежжи
		bullet.ConstantForce = (GetGlobalMousePosition() - bullet.GlobalPosition) / 
		                        (GetGlobalMousePosition() - bullet.GlobalPosition).Length();
		bullet.ConstantForce *= ProjectileVelocity; // speed here ежжи
		bullet.Set("Target", (int)Target);
		bullet.Set("Damage", DamageBase);  // todo: add more complex damage system (dmg per sec)
		AddChild(bullet);
	}
}