using Godot;

public class MoveOrder : Order {
	public MoveOrder(Vector2 position, Node parent) : base(parent, BehaviorTreeConfig.Trees.Move) {
		context = this;
		GlobalPosition = position;
		Name = GetType().Name;
		GD.Print(string.Format("New Move Order at {0}", position));
	}

}
