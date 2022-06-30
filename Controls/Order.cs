using Godot;

public abstract class Order : Node2D {

	public Order(Node scope) {
		scope.AddChild(this, legibleUniqueName:true);
	}

	int _participants = 0;

	public int participants { get => _participants; set {
		_participants = value;
		if (participants == 0){
			QueueFree();
		}
	}}

	//Returns true if order was sucessfully started for data, false if it was rejected
	public abstract bool Start(Node data);

	//Return true to indicate execution should end for data
	public abstract bool Update(Node data);

	public abstract void Stop(Node data);
}
