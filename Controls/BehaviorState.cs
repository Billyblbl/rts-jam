using Godot;

public class BehaviorState : State {

	public BehaviorState(Node2D actor) {
		this.actor = actor;
	}

	public Node2D actor;

}
