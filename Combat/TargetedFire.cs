using Godot;

public class TargetedFire : BehaviorState {

	void UpdateTarget(Node2D target) {
		var weapon = actor.GetNode<Weapon>(nameof(Weapon));
		weapon.LookAt(target.GlobalPosition);

		var anim = actor.GetNodeOrNull<DirectionalAnimatedSprite>(nameof(DirectionalAnimatedSprite));
		anim.direction = actor.GlobalPosition.DirectionTo(target.GlobalPosition);
		anim.state = this;
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

		var anim = actor.GetNodeOrNull<DirectionalAnimatedSprite>(nameof(DirectionalAnimatedSprite));
		if (anim != null) {
			anim.state = null;
		}

		base.Exitstate();
	}

}
