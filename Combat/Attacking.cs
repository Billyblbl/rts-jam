using Godot;

public class Attacking : BehaviorState {

	public bool targetInRange(Node2D target) => actor.GlobalPosition.DistanceTo(target.GlobalPosition) < actor.GetNode<Weapon>(nameof(Weapon)).attackRange;

	public Attacking(Node2D actor, Node2D target) : base(actor) {
		AddChild(new Follow(actor, target));
	}

	public override void UpdateState<T>(T context) => UpdateAttack(context as Node2D);

	public void UpdateAttack(Node2D target) {
		var subState = currentSubState;
		if (subState is Follow && targetInRange(target)) {
			TransitionTo(() => new TargetedFire(actor, target));
		} else if (subState is TargetedFire && !targetInRange(target)) {
			TransitionTo(() => new Follow(actor, target));
		}
		subState.UpdateState(target);
	}

}
