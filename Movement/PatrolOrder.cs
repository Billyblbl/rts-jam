using System.Collections.Generic;
using Godot;

public class PatrolOrder : Order {

	public List<Vector2> waypoints;
	public PatrolOrder(List<Vector2> waypoints) : base(actor => new Patroling(actor, waypoints)) {
		Name = GetType().Name;
		this.waypoints = waypoints;
	}

	public override bool Update(Controllable actor) {
		try {
			actor.currentBehavior.UpdateState(waypoints);
			return true;
		} catch (System.Exception e) {
			GD.PrintErr(e);
			return false;
		}
	}

}
