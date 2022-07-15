using Godot;
using System.Linq;
using System.Collections.Generic;

public class PathFindAgent : Node2D {

	[Export] public Slot<Navigation2D> navSlot = null;
	[Export] public NodePath body = null;
	[Export] public float speed = 1f;
	[Export] public float waypointThreshold = float.Epsilon;
	[Export] public float seekDistance = 100;
	// [Export] public float acceleration;
	public Vector2[] path = null;
	public int currentWaypoint = 0;
	KinematicBody2D self = null;

	[Export] NodePath[] steeringSensorsLeft;
	[Export] NodePath[] steeringSensorsRight;

	public ProximitySensor[] steeringSensorsLeftNodes;
	public ProximitySensor[] steeringSensorsRightNodes;

	public CollisionPolygon2D steeringConeNode;

	public void StopPathing() {
		currentWaypoint = 0;
		path = null;
	}

	public Vector2 _targetDestination = Vector2.Zero;
	public Vector2 targetDestination {
		get => _targetDestination; set {
			_targetDestination = value;
			var nav = navSlot?.instance;
			if (nav != null && self != null) {
				GD.Print(string.Format("Pathing from {0} to {1}", self.GlobalPosition, targetDestination));
				path = nav.GetSimplePath(self.GlobalPosition, targetDestination);
				currentWaypoint = 0;
				if (GlobalPosition.DistanceSquaredTo(path[currentWaypoint]) < waypointThreshold)
					currentWaypoint++;
				// GD.Print(string.Format("Path : {0}", path));
			} else {
				path = null;
				currentWaypoint = 0;
			}
		}
	}

	public float manhattanDistanceToDestination { get => self != null ? (self.Position - targetDestination).LengthSquared() : 0f; }

	public override void _Ready() {
		self = GetNodeOrNull<KinematicBody2D>(body) ?? throw new System.Exception(string.Format("No body found for {0}", Name));
		steeringSensorsLeftNodes = steeringSensorsLeft.Select(p => GetNode<ProximitySensor>(p)).ToArray();
		steeringSensorsRightNodes = steeringSensorsRight.Select(p => GetNode<ProximitySensor>(p)).ToArray();
	}

	Vector2 velocity;

	float Participation(RayCast2D ray) {
		if (ray.IsColliding()) {
			return ray.GetCollisionPoint().DistanceSquaredTo(GlobalPosition) / ray.CastTo.Length() * ray.CastTo.Length();
		} else {
			return 1f;
		}
	}

	Vector2 UnitForward { get => Vector2.Right.Rotated(GlobalRotation); }
	Vector2 UnitLeft { get => Vector2.Up.Rotated(GlobalRotation); }
	Vector2 UnitRight { get => Vector2.Down.Rotated(GlobalRotation); }


	Vector2 TowardsSegment(Vector2 p1, Vector2 p2) {
		var intersect = Geometry.SegmentIntersectsCircle(p2, p1, GlobalPosition, seekDistance);
		if (intersect < 0) {
			return GlobalPosition.DirectionTo(p1);
		} else {
			return GlobalPosition.DirectionTo(p2.LinearInterpolate(p1, intersect));
		}
	}

	public bool atDestination { get => GlobalPosition.DistanceSquaredTo(targetDestination) < waypointThreshold * waypointThreshold; }

	public override void _PhysicsProcess(float delta) {
		if (path == null || currentWaypoint >= path.Length) return;

		if (GlobalPosition.DistanceSquaredTo(path[currentWaypoint]) < waypointThreshold * waypointThreshold) {
			currentWaypoint++;
		} else {

			var direction = (currentWaypoint < path.Length - 1 ? TowardsSegment(path[currentWaypoint], path[currentWaypoint + 1]) : GlobalPosition.DirectionTo(path[currentWaypoint]));
			GlobalRotation = direction.Angle();

			Vector2 steered;
			if (
				steeringSensorsLeftNodes.Any(s => s.IsColliding()) ||
				steeringSensorsRightNodes.Any(s => s.IsColliding())
			) {
				var steerLeft = steeringSensorsLeftNodes
					.Select(s => s.globalDirection * s.rangeProportion)
					.Aggregate(Vector2.Zero, (v1, v2) => v1 + v2);

				steerLeftCache = steerLeft;

				var steerRight = steeringSensorsRightNodes
					.Select(s => s.globalDirection * s.rangeProportion)
					.Aggregate(Vector2.Zero, (v1, v2) => v1 + v2);

				steerRightCache = steerRight;

				if (steerLeft.LengthSquared() > steerRight.LengthSquared()) {
					steered = steerLeft.Normalized();
				} else {
					steered = steerRight.Normalized();
				}

			} else {
				steered = UnitForward;
				steerLeftCache = Vector2.Zero;
				steerRightCache = Vector2.Zero;
			}

			// GD.Print(string.Format("Intended direction = {0}, Steered = {1}, steered angle {2}", direction, steered, Mathf.Rad2Deg(direction.AngleTo(steered))));
			velocity = steered * Mathf.Clamp(speed, 0, manhattanDistanceToDestination);
			// velocity = direction * Mathf.Clamp(speed, 0, manhattanDistanceToDestination);
			self.MoveAndSlide(velocity);
#if !GODOT_EXPORT
			trace.Add(GlobalPosition);
#endif
		}
	}

#if !GODOT_EXPORT

	//!Debug
	public List<Vector2> trace = new List<Vector2>();

	Vector2 steerLeftCache;
	Vector2 steerRightCache;

	public override void _Process(float delta) {
		base._Process(delta);
		Update();
	}

	[Export] Gradient rayProximity;

	public override void _Draw() {
		base._Draw();

		foreach (var ray in steeringSensorsLeftNodes) {
			DrawLine(Position, ToLocal(ray.ToGlobal(ray.CastTo)), Colors.Cyan, width: 1);
		}

		foreach (var ray in steeringSensorsRightNodes) {
			DrawLine(Position, ToLocal(ray.ToGlobal(ray.CastTo)), Colors.Cyan, width: 1);
		}

		if (path != null) {
			foreach (var (point, i) in path.Select((p, i) => (p, i))) {
				if (i == 0)
					DrawLine(Position, ToLocal(point), Colors.Blue, width: 1);
				else
					DrawLine(ToLocal(path[i - 1]), ToLocal(point), Colors.Green, width: 1);
			}
		}

		// DrawCircle(Position, waypointThreshold, Colors.Red);
		DrawArc(Position, waypointThreshold, 0, 2 * Mathf.Pi, 10, Colors.Red);
		DrawArc(Position, seekDistance, 0, 2 * Mathf.Pi, 10, Colors.Blue);

		DrawLine(Position, ToLocal(GlobalPosition + velocity * 2), Colors.Red, width: 3);
		DrawLine(Position, ToLocal(GlobalPosition + steerLeftCache * 2), Colors.Aquamarine, width: 3);
		DrawLine(Position, ToLocal(GlobalPosition + steerRightCache * 2), Colors.Aquamarine, width: 3);
	}

#endif

}
