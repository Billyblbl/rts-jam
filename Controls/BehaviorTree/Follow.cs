using Godot;

public class Follow : BehaviorState {

	[Export] public float repathThreshold = 10f;
	[Export] public NodePath move;
	Move moveState;

	public bool targetMoved((Node2D context, Node2D actor) data) {
		var pathfind = data.actor.GetNode<PathFindAgent>(typeof(PathFindAgent).Name);
		var target = data.context as Node2D;
		return pathfind.targetDestination.DistanceTo(target.GlobalPosition) > repathThreshold;
	}

	public override void _Ready() {
		base._Ready();
		moveState = GetNode<Move>(move);

		subStates.connections = new BehaviorStateMachine.Connection[] {
			new BehaviorStateMachine.Connection ( "Target moved", moveState, moveState, (data) => targetMoved(data) )
		};
	}

	public override void EnterState((Node2D context, Node2D actor) data) {
		base.EnterState(data);
		moveState.EnterState(data);
	}

	public override void ExitState((Node2D context, Node2D actor) data) {
		base.ExitState(data);
	}

}
