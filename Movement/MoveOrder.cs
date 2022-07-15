using Godot;

public class MoveOrder : Order {
	public MoveOrder(Vector2 position) : base(actor => new Move(actor, position)) {
		Name = GetType().Name;
		GD.Print(string.Format("New Move Order at {0}", position));
	}

	public override bool Update(Controllable actor) {
		try {
			actor.currentBehavior.UpdateState(this);
			return !actor.bodyNode.GetNode<PathFindAgent>(nameof(PathFindAgent)).atDestination;
		} catch (System.Exception e) {
			GD.PrintErr(e);
			return false;
		}
	}

}
