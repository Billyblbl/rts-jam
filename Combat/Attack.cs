using Godot;

public class Attack : RigidBody2D {
	[Export] public float damage;
	[Export] public float speed;
	[Export] public Vector2 direction = Vector2.Right;

	public void Launch() {
		LinearVelocity = direction.Rotated(GlobalRotation) * speed;
	}

}
