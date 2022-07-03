using Godot;

public class Attacking : BehaviorState {

	[Export] public NodePath follow;
	Follow followState;
	[Export] public NodePath targetedFire;
	TargetedFire targetedFireState;

	public override void _Ready() {
		base._Ready();

		followState = GetNode<Follow>(follow);
		targetedFireState = GetNode<TargetedFire>(targetedFire);

		subStates.connections = new BehaviorStateMachine.Connection[] {
			new BehaviorStateMachine.Connection ( "Target in range", followState, targetedFireState, (data) => targetInRange(data) ),
			new BehaviorStateMachine.Connection ( "Target out of range", targetedFireState, followState, (data) => !targetInRange(data) )
		};
	}

	public bool targetInRange((Node2D context, Node2D actor) data) {
		var weapon = data.actor.GetNode<Weapon>(typeof(Weapon).Name);

		// GD.Print(string.Format("Distance to target = {0}, required under {1}", data.context.GlobalPosition.DistanceTo(data.actor.GlobalPosition), weapon.attackRange));

		return data.context.GlobalPosition.DistanceTo(data.actor.GlobalPosition) < weapon.attackRange;
	}

	public override void EnterState((Node2D context, Node2D actor) data) {
		base.EnterState(data);
		followState.EnterState(data);
	}

	public override void ExitState((Node2D context, Node2D actor) data) {
		base.ExitState(data);
	}

}
