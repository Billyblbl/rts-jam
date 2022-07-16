using Godot;

public class Attacking : Targetting<TargetedFire> {
	public Attacking(Node2D actor, Node2D target) : base(actor, target) { }
	public override void OnTargetInRange(Node2D target) => TransitionTo(() => new TargetedFire(actor, target));
	public override bool TargetInRange(Node2D target) => actor.GlobalPosition.DistanceTo(target.GlobalPosition) < actor.GetNode<Weapon>(nameof(Weapon)).attackRange;
}
