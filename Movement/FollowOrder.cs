using Godot;
using System.Linq;

public class FollowOrder : Order {

	public Node2D target;

	public FollowOrder(Node2D target, Node scope) : base(scope) {
		this.target = target;
		GlobalPosition = target.Position;
		Name = GetType().Name;
		GD.Print(string.Format("New Follow Order towards {0}", target.Name));
	}

	public override bool Start(Node data) {
		if (data == target || data.IsAParentOf(target)) return false;
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
		if (IsInstanceValid(target)) {
			pathfind.targetDestination = GlobalPosition;
			return false;
		} else return true;
	}

	public override void _Process(float delta) {
		//// consider just putting the follow order as a child of the target instead
		//Actually don't do what the comment above says, could potentially produce situation where the order is destroyed before Stop is called
		if (IsInstanceValid(target)) GlobalPosition = target.GlobalPosition;
	}
}
