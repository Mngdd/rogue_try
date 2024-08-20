namespace rogue_try.Code.Weapons;

using Godot;
using System;

public partial class WeaponFirearm : WeaponBase {
	[Export] public int AmmoMax = 0;
	[Export] public int AmmoCurrent = 0;
	[Export] public int ProjectileVelocity = 250;
	[Export] public double ReloadTime = 1d;
	[Export] private protected PackedScene BulletScene;  // DELME OR FIXME!!!!!
	
	private protected Timer ReloadTimer = new Timer();
	private protected bool CanShootReload = true;

	
	public override void _Ready() {
		base._Ready();
		
		AddChild(ReloadTimer);
		ReloadTimer.WaitTime = ReloadTime;
		ReloadTimer.OneShot = true;
		Callable onReloadEnd = Callable.From(() => { AmmoCurrent = AmmoMax; CanShootReload = true;});
		ReloadTimer.Connect("timeout", onReloadEnd);
	}
	
	private protected override bool _canShoot() {
		return base._canShoot() && CanShootReload;
	}
	
  	// public override void Fire() {
  	// 	if (AmmoCurrent == 0) {
  	// 		Reaload();
  	// 		return;
  	// 	}
  	// 	
  	// 	--AmmoCurrent;
  	// 	// TODO: sound?
	  //   RigidBody2D bullet = _bulletScene.Instantiate<RigidBody2D>();
	  //   var shooter = GetParent<CharacterBody2D>();
	  //   shooter.Call("Kill");
	  //   GD.Print("ЭТО РОФЛССС!!! :D");
	  //   // bullet.Rotation = GetAngleTo(GetGlobalMousePosition());
	  //   // bullet.GlobalPosition = GlobalPosition;
	  //   // bullet.LinearVelocity = bullet.Transform.X * 200;
			//   //
	  //   // GetTree().Root.AddChild(bullet);
   //  }
	
	public override void Reaload() {
		ReloadTimer.Start();
	}
}