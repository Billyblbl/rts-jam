using Godot;
using System.Linq;

public class MoveOrder : Order {

	public MoveOrder(Vector2 position, Node scope) : base(scope) {
		GlobalPosition = position;
		Name = GetType().Name;
		GD.Print(string.Format("New Move Order at {0}", position));
	}

	public override bool Start(Node data) {
		var pathfind = data.GetNodeOrNull<PathFindAgent>(typeof(PathFindAgent).Name);
		if (pathfind == null) return false;
		pathfind.targetDestination = GlobalPosition;
		pathfind.SetProcess(true);
		return true;
	}

	public override void Stop(Node data) {
		var pathfind = data.GetNodeOrNull<PathFindAgent>(typeof(PathFindAgent).Name);
		if (pathfind == null) return;
		pathfind.SetProcess(false);
	}

	public override bool Update(Node data) {
		var pathfind = data.GetNodeOrNull<PathFindAgent>(typeof(PathFindAgent).Name);
		if (pathfind == null) return true;
		return pathfind.manhattanDistanceToDestination <= float.Epsilon;
	}
}
