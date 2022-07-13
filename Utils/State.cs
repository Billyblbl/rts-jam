using Godot;
using System.Linq;

// #nullable enable

public class State : Node2D {

	public State currentSubState { get => GetChildren().Cast<Node>().FirstOrDefault(node => node is State) as State; }
	public T GetCurrentSubState<T>() where T : State => GetChildren().Cast<Node>().FirstOrDefault(node => node is T) as T;

	public State() => Name = GetType().Name;

	public void Clear() {
		foreach (var child in GetChildren().Cast<Node>()) {
			if (child is State state) state.Exitstate();
		}
	}

	public T TransitionTo<T>(System.Func<T> stateFactory) where T : State {
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

	public override void _ExitTree() {
		Exitstate();
		base._ExitTree();
	}

}
