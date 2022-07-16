using Godot;

public class RepairOrder : Order {

	public Node2D target;

	public RepairOrder(Node2D target) : base(actor => new BuildingState(actor, target)) {
		this.target = target;
		Name = GetType().Name;
		GD.Print(string.Format("New Repair Order on {0}", target.Name));
	}

	public override bool Start(Controllable actor) {
		if (target.IsAParentOf(actor) || actor.IsAParentOf(target))
			return false;
		else
			return base.Start(actor);
	}

	public override bool Update(Controllable actor) {
		if (target == null || !Node.IsInstanceValid(target)) return false;
		var targetHealth = target.GetNode<Health>(nameof(Health));
		if (targetHealth.value.current >= targetHealth.value.max) return false;
		actor.currentBehavior.UpdateState(target);
		return true;
	}
}
