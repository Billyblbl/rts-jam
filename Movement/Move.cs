using Godot;

public class Move : BehaviorState {

	public Move(Node2D actor, Vector2 position) : base(actor) {
		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
		pathfind.targetDestination = position;
	}

	public override void Exitstate() {
		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
		pathfind.StopPathing();
		base.Exitstate();
	}

}
