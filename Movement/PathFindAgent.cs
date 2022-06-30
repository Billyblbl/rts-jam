using Godot;

public class PathFindAgent : Node2D {

	[Export] public Slot<Navigation2D> navSlot = null;
	[Export] public NodePath body = null;
	[Export] public float speed = 1f;
	[Export] public float waypointThreshold = float.Epsilon;
	// [Export] public float acceleration;

	Vector2[] path = null;
	int currentWaypoint = 0;
	KinematicBody2D self = null;

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
	}

	public override void _Process(float delta) {
		if (path == null || currentWaypoint >= path.Length) return;

		if (GlobalPosition.DistanceSquaredTo(path[currentWaypoint]) < waypointThreshold) {
			currentWaypoint++;
		} else {
			var direction = GlobalPosition.DirectionTo(path[currentWaypoint]);
			var velocity = direction * speed;
			self.MoveAndSlide(velocity);
		}
	}

}
