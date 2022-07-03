using Godot;

public class AttackOrder : Order {
	public AttackOrder(Node2D target, Node parent) : base(parent, BehaviorTreeConfig.Trees.Attacking) {
		context = target;
		Name = GetType().Name;
		GD.Print(string.Format("New Attack Order on {0}", target.Name));
	}

	public override bool Start(Node2D actor) {
		if (
			!usableContext ||
			context == actor ||
			context.IsAParentOf(actor) ||
			actor.IsAParentOf(context)
		)
			return false;
		else
			return base.Start(actor);
	}

	public override bool Update(Node2D actor) {
		if (context == null || !Node.IsInstanceValid(context)) return false;
		return base.Update(actor);
	}
}
