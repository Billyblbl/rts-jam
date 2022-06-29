using Godot;

public abstract class Order : Node2D {

	public Order(Node scope) {
		scope.AddChild(this, legibleUniqueName:true);
	}

	int _participants = 0;

	public int participants { get => _participants; set {
		_participants = value;
		GD.Print(string.Format("Order {0}, participants {1}", Name, participants));
		if (participants == 0){
			GD.Print("order finished, buebye");
			QueueFree();
		}
	}}

	public abstract void Start(Node data);

	//Return true to indicate execution should end
	public abstract bool Update(Node data);

	public abstract void Stop(Node data);
}
