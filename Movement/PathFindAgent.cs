using Godot;

public class PathFindAgent : Node2D {

	[Export] public Slot<Navigation2D> navSlot = null;
	[Export] public NodePath body = null;
	[Export] public float speed = 1f;
	[Export] public float waypointThreshold = float.Epsilon;
	// [Export] public float acceleration;

	Vector2[]	path = null;
	int currentWaypoint = 0;
	KinematicBody2D self = null;
	Node2D _target = null;
	public Node2D target { get => _target; set {
		GD.Print(string.Format("new target {0}", value));
		_target = value;
		var nav = navSlot?.instance;
		if (nav != null && self != null && target != null) {
			path = nav.GetSimplePath(self.Position, target.Position);
			currentWaypoint = 0;
		} else {
			path = null;
			currentWaypoint = 0;
		}
	}}
	public float manhattanDistanceToDestination { get => self != null && target != null ? (self.Position - target.Position).LengthSquared() : 0f; }

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
