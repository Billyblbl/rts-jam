using Godot;


public class ProximitySensor : RayCast2D {

	public float distanceSquared;
	public float distance { get => distanceSquared < 0f ? -1f : Mathf.Sqrt(distanceSquared); }
	public float rangeProportion { get => distanceSquared < 0f ? 1f : distanceSquared / CastTo.LengthSquared(); }
	public Vector2 globalDirection { get => GlobalPosition.DirectionTo(ToGlobal(CastTo)); }

	public override void _PhysicsProcess(float delta) {
		base._PhysicsProcess(delta);

		if (IsColliding()) {
			distanceSquared = GetCollisionPoint().DistanceSquaredTo(GlobalPosition);
		} else {
			distanceSquared = -1;
		}

	}
}
