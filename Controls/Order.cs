using Godot;

public abstract class Order : Node2D {

	public Order(Node parent, BehaviorState behaviorTree) {
		parent.AddChild(this, legibleUniqueName:true);
		this.behaviorTree = behaviorTree;
	}

	public BehaviorState behaviorTree;

	int _participants = 0;
	public int participants { get => _participants; set {
		_participants = value;
		if (participants == 0) QueueFree();
	}}

	public Node2D context;

	public virtual bool Start(Node2D actor) {
		behaviorTree.EnterState((context ?? this, actor));
		return true;
	}
	public virtual bool Update(Node2D actor) {
		behaviorTree.StayState((context ?? this, actor));
		return true;
	}
	public virtual void Stop(Node2D actor) => behaviorTree.ExitState((this, actor));

}
