using Godot;
using System.Linq;

public class BehaviorState : Node {

	public BehaviorState(Node2D actor) {
		this.actor = actor;
		Name = GetType().Name;
	}

	public Node2D actor;

	public BehaviorState currentSubState { get => GetChildren().Cast<Node>().FirstOrDefault(node => node is BehaviorState) as BehaviorState; }

	public void Clear() {
		foreach (var child in GetChildren().Cast<Node>()) {
			if (child is BehaviorState state) state.Exitstate();
		}
	}

	public T TransitionTo<T>(System.Func<T> stateFactory) where T : BehaviorState {
		Clear();
		var state = stateFactory.Invoke();
		AddChild(state);
		return state;
	}

	public virtual void UpdateState<T>(T context) { currentSubState?.UpdateState(context); }
	public virtual void Exitstate() {
		Clear();
		QueueFree();
	}

}
