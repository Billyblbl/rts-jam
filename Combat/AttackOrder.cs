using Godot;

public class AttackOrder : Order {

	public Node2D target;
	public AttackOrder(Node2D target, Node parent) : base(parent, actor => new Attacking(actor, target)) {
		this.target = target;
		Name = GetType().Name;
		GD.Print(string.Format("New Attack Order on {0}", target.Name));
	}

	public override bool Start(Controllable actor) {
		if (target.IsAParentOf(actor) || actor.IsAParentOf(target))
			return false;
		else
			return base.Start(actor);
	}

	public override bool Update(Controllable actor) {
		//TODO intel check
		if (target == null || !Node.IsInstanceValid(target)) return false;
		actor.currentBehavior.UpdateState(target);
		return true;
	}
}
