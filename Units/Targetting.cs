using Godot;

public abstract class Targetting<S> : BehaviorState where S : BehaviorState {

	public abstract bool TargetInRange(Node2D target);
	public abstract void OnTargetInRange(Node2D target);

	public Targetting(Node2D actor, Node2D target) : base(actor) {
		AddChild(new Follow(actor, target));
	}

	public override void UpdateState<T>(T context) => UpdateAttack(context as Node2D);

	public void UpdateAttack(Node2D target) {
		var subState = currentSubState;
		GD.Print(string.Format("Distance To Target {0}, attack under {1}", actor.GlobalPosition.DistanceTo(target.GlobalPosition), actor.GetNode<Weapon>(nameof(Weapon)).attackRange));
		if (subState is Follow && TargetInRange(target)) {
			// TransitionTo(() => new TargetedFire(actor, target));
		} else if (subState is S && !TargetInRange(target)) {
			TransitionTo(() => new Follow(actor, target));
		}
		subState.UpdateState(target);
	}

}
