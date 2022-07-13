using Godot;

using BehaviorStateFactory = System.Func<Godot.Node2D, BehaviorState>;

public abstract class Order : Node {

	protected BehaviorStateFactory factory;
	public Order(BehaviorStateFactory stateFactory = null) {
		factory = stateFactory;
	}

	int _participants = 0;
	public int participants { get => _participants; set {
		_participants = value;
		if (participants == 0) QueueFree();
	}}

	public virtual bool Start(Controllable actor) {
		try {
			var newNode = factory.Invoke(actor.GetParent<Node2D>());
			actor.currentBehavior.AddChild(newNode);
			return true;
		} catch (System.Exception e) {
			GD.PrintErr(e);
			return false;
		}
	}

	public abstract bool Update(Controllable actor);

	public virtual void Stop(Controllable actor) {
		try {
			actor.currentBehavior.Clear();
		} catch (System.Exception e) {
			GD.PrintErr(e);
		}
	}

}
