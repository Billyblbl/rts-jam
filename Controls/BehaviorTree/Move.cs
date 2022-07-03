using Godot;

public class Move : BehaviorState {
	public override void EnterState((Node2D context, Node2D actor) data) {
		base.EnterState(data);
		var pathfind = data.actor.GetNode<PathFindAgent>(typeof(PathFindAgent).Name);
		pathfind.targetDestination = data.context.GlobalPosition;
		pathfind.SetProcess(true);
	}

	public override void ExitState((Node2D context, Node2D actor) data) {
		base.ExitState(data);
		var pathfind = data.actor.GetNodeOrNull<PathFindAgent>(typeof(PathFindAgent).Name);
		if (pathfind == null) return;
		pathfind.SetProcess(false);
	}

}
