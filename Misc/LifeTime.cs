using Godot;

public class LifeTime : Timer {

	[Export] public NodePath scope;
	public override void _Ready() {
		base._Ready();
		Connect("timeout", GetNode(scope) ?? this, "queue_free");
	}
}
