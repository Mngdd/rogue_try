namespace rogue_try.Code.Weapons;

using Godot;

public partial class GunPistol : GunBase {
	public override void _Ready() {
		base._Ready();
		AmmoUsed = AmmoTypes.Pistol;
	}

	public override void Fire() {
		if (AmmoCurrent == 0) {
			base.Reaload();
			return;
		}
		
		--AmmoCurrent;
		// spawn projectile, and give it damage and etc...
		// TODO: sound?

	}
}