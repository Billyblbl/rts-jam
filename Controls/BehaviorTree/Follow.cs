using Godot;

public class Follow : BehaviorState {

	public static float repathThreshold = 10f;

	public Follow(Node2D actor, Node2D target) : base(actor) {
		AddChild(new Move(actor, target.GlobalPosition));
	}

	public bool targetMoved(Vector2 position) {
		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
		return pathfind.targetDestination.DistanceTo(position) > pathfind.waypointThreshold;
	}

	public override void UpdateState<T>(T context) => UpdateTarget(context as Node2D);

	void UpdateTarget(Node2D target) {
		if (targetMoved(target.GlobalPosition)) {
			TransitionTo(() => new Move(actor, target.GlobalPosition));
		}
	}

}
