using Godot;

public class NodeProvider<T> : Node where T : Node {
	[Export] public Slot<T> slot;
	[Export] public NodePath instance;

	public override void _Ready() {
		slot.instance = GetNode<T>(instance);
	}
}
