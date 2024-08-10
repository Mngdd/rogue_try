namespace rogue_try.Code.Weapons;

using Godot;
using System;

public partial class GunBase : WeaponBase {
	[Export] public int AmmoMax = 0;
	[Export] public int AmmoCurrent = 0;
	[Export] public double ReloadTime = 1d;
	[Export] public double FireInterval = 1d;
	[Export] private PackedScene bullet_scn;
	private Timer _reloadTimer = new Timer();
	
	[Export(PropertyHint.Enum, "Pistol,Sniper,Shotgun,AssRifle,Explosive,Energy")]
	public AmmoTypes AmmoUsed = AmmoTypes.Pistol; // чем стреляем

	public enum AmmoTypes {
		Pistol,
		Sniper,
		Shotgun,
		AssRifle,
		Explosive,
		Energy
	}
	
	
	public override void _Ready() {
		_reloadTimer.WaitTime = ReloadTime;
		_reloadTimer.OneShot = true;
		
		//bullet_scn = ResourceLoader.Load<PackedScene>("res:// scene. tscn").Instantiate();
	}
	
	public void Reaload() {
		Callable callable = Callable.From(() => AmmoCurrent = AmmoMax);
		// TODO: PLAY SOUND OR SOMETHING
		_reloadTimer.Connect("timeout", callable);
		_reloadTimer.Start();
	}
}