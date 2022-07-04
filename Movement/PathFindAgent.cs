using Godot;
using System.Linq;
using System.Collections.Generic;

public class PathFindAgent : Node2D {

	[Export] public Slot<Navigation2D> navSlot = null;
	[Export] public NodePath body = null;
	[Export] public float speed = 1f;
	[Export] public float waypointThreshold = float.Epsilon;
	// [Export] public float acceleration;
	[Export] public NodePath[] steeringRays;
	[Export] public NodePath raysPivot;
	public RayCast2D[] steeringRayNodes;
	public Node2D raysPivotNode;

	Vector2[] path = null;
	int currentWaypoint = 0;
	KinematicBody2D self = null;

	public void StopPathing() => path = null;

	public Vector2 _targetDestination = Vector2.Zero;
	public Vector2 targetDestination {
		get => _targetDestination; set {
			_targetDestination = value;
			var nav = navSlot?.instance;
			if (nav != null && self != null) {
				path = nav.GetSimplePath(self.Position, targetDestination);
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
		self = ((body != null) && !body.IsEmpty() ? GetNode<KinematicBody2D>(body) : throw new System.Exception(string.Format("No body found for {0}", Name)));
		steeringRayNodes = steeringRays.Select(p => GetNode<RayCast2D>(p)).ToArray();
		raysPivotNode = GetNode<Node2D>(raysPivot);
		foreach (var ray in steeringRayNodes) {
			ray.AddException(self);
		}
	}

	Vector2 velocity;

	float Participation(RayCast2D ray) {
		if (ray.IsColliding()) {
			return ray.GetCollisionPoint().DistanceTo(GlobalPosition) / ray.CastTo.Length();
		} else {
			return 1f;
		}
	}

	Vector2 UnitForward { get => Vector2.Right.Rotated(GlobalRotation); }
	Vector2 UnitLeft { get => Vector2.Up.Rotated(GlobalRotation); }
	Vector2 UnitRight { get => Vector2.Down.Rotated(GlobalRotation); }

	public override void _PhysicsProcess(float delta) {
		if (path == null || currentWaypoint >= path.Length) return;

		if (GlobalPosition.DistanceSquaredTo(path[currentWaypoint]) < waypointThreshold) {
			currentWaypoint++;
		} else {
			var direction = GlobalPosition.DirectionTo(path[currentWaypoint]);
			//!@Issue this shit is way too much code for what it does, big chance its way too slow
			GlobalRotation = direction.Angle();

			Vector2 RayDirection(RayCast2D r) => r.GlobalPosition.DirectionTo(r.ToGlobal(r.CastTo));

			Vector2 Steered(IEnumerable<RayCast2D> collection) => collection
				.Select(r => RayDirection(r) * Participation(r))
				.Aggregate((a, b) => a + b)
				.Normalized();

			Vector2 steered;

			var middleRay = steeringRayNodes
				.OrderByDescending(r => RayDirection(r)
					.Project(UnitForward)
					.Length())
				.ToArray()[0];

			IEnumerable<RayCast2D> OrientedProportionRays(Vector2 orientation, float proportion) => steeringRayNodes
				.OrderBy(r => RayDirection(r)
					.Project(orientation)
					.Length())
				.Where((r,i) => i <(float)steeringRayNodes.Length*proportion);

			if (middleRay.IsColliding()) {
				steered = new Vector2[] {
					Steered(OrientedProportionRays(UnitLeft, .5f)),
					Steered(OrientedProportionRays(UnitRight, .5f))
				}.OrderByDescending(v => v.Length()).ToArray()[0];
			} else {
				steered = UnitForward;
			}

			// GD.Print(string.Format("Intended direction = {0}, Steered = {1}, steered angle {2}", direction, steered, Mathf.Rad2Deg(direction.AngleTo(steered))));
			velocity = steered * Mathf.Clamp(speed, 0, manhattanDistanceToDestination);
			self.MoveAndSlide(velocity);
		}
	}

#if !GODOT_EXPORT

	public override void _Process(float delta) {
		base._Process(delta);
		Update();
	}

	[Export] Gradient rayProximity;

	public override void _Draw() {
		base._Draw();
		foreach (var ray in steeringRayNodes) {
			DrawLine(ToLocal(ray.GlobalPosition), ToLocal(ray.ToGlobal(ray.CastTo)), rayProximity.Interpolate(Participation(ray)));
		}
		DrawLine(Position, ToLocal(GlobalPosition + velocity), Colors.Red, width: 1);

		if (path != null) foreach (var (point, i) in path.Select((p, i) => (p, i))) {
				if (i == 0)
					DrawLine(Position, ToLocal(point), Colors.Blue, width: 1);
				else
					DrawLine(ToLocal(path[i - 1]), ToLocal(point), Colors.Green, width: 1);
			}
	}

#endif

}
