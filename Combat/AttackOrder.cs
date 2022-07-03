using Godot;

public class AttackOrder : Order {
	public AttackOrder(Node2D target, Node parent) : base(parent, BehaviorTreeConfig.Trees.Attacking) {
		context = target;
		Name = GetType().Name;
		GD.Print(string.Format("New Attack Order on {0}", target.Name));
	}
}
