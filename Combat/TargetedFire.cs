using Godot;

public class TargetedFire : BehaviorState {

	void UpdateTarget(Node2D target) {
		var weapon = actor.GetNode<Weapon>(nameof(Weapon));
		weapon.LookAt(target.GlobalPosition);
	}

	public override void UpdateState<T>(T context) => UpdateTarget(context as Node2D);

	public TargetedFire(Node2D actor, Node2D target) : base(actor) {
		var weapon = actor.GetNode<Weapon>(nameof(Weapon));
		weapon.LookAt(target.GlobalPosition);
		weapon.attacking = true;
	}

	public override void Exitstate() {
		var weapon = actor.GetNode<Weapon>(nameof(Weapon));
		weapon.attacking = false;
		base.Exitstate();
	}

}
