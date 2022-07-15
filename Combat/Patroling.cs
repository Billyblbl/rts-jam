using Godot;
using System.Linq;
using System.Collections.Generic;
public class Patroling : BehaviorState {

	int currentWayPointIndex;
	Node2D target;

	public Patroling(Node2D actor, List<Vector2> waypoints) : base(actor) {
		AddChild(new Move(actor, waypoints[0]));
		currentWayPointIndex = 0;
	}

	int NextIndex(List<Vector2> waypoints) => currentWayPointIndex < waypoints.Count - 1 ? ++currentWayPointIndex : currentWayPointIndex = 0;
	int CurrentIndex(List<Vector2> waypoints) => currentWayPointIndex < waypoints.Count ? currentWayPointIndex : currentWayPointIndex = 0;

	public override void UpdateState<T>(T context) => Update(context as List<Vector2>);

	private void Update(List<Vector2> waypoints) {
		var subState = currentSubState;
		if (subState is Move) {
			var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
			if (EnemyInSight(out target)) {
				TransitionTo(() => new Attacking(actor, target));
			} else if (pathfind.atDestination) {
				TransitionTo(() => new Move(actor, waypoints[NextIndex(waypoints)]));
			}
		} else if (subState is Attacking) {
			if (!EnemyInSight(out var _)) {
				TransitionTo(() => new Move(actor, waypoints[CurrentIndex(waypoints)]));
			} else {
				subState.UpdateState(target);
			}
		}
	}

	public bool EnemyInSight(out Node2D enemy) {
		var sight = actor.GetNode<Sight>(nameof(Sight));
		if (sight.EnemyInSight.Count > 0) {
			enemy = sight.EnemyInSight
				.OrderBy(e => e.bodyNode.GlobalPosition.DistanceSquaredTo(actor.GlobalPosition))
				.Take(1)
				.Single()
				.bodyNode;
			return true;
		} else {
			enemy = null;
			return false;
		}
	}


}
