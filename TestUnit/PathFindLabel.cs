using Godot;
using System.Linq;

public class PathFindLabel : Label {
	[Export] NodePath pathfind;

	PathFindAgent agent;

	public override void _Ready() {
		base._Ready();
		agent = GetNode<PathFindAgent>(pathfind);
	}

	public override void _Process(float delta) {
		base._Process(delta);
		if (agent.path != null && agent.path.Length > 0 && agent.currentWaypoint < agent.path.Length) {
			Text = string.Concat(
				string.Format("Path : {0}\nWaypoint index : {1}\nWaypoint : {2}\nPosition : {3}",
					agent.path.Aggregate("V2", (a,b) => string.Format("{0}:{1}", a, b)),
					agent.currentWaypoint,
					agent.path?[agent.currentWaypoint],
					agent.GlobalPosition
				),
				string.Format("Distance To Waypoint {0}\n", agent.GlobalPosition.DistanceTo(agent.path?[agent.currentWaypoint] ?? agent.GlobalPosition))
			);
		} else Text = "";
		// Update();
	}

}
