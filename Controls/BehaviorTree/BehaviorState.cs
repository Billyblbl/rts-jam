using Godot;
using System.Linq;

public abstract class BehaviorState : Node, State<(Node2D context, Node2D actor)> {

	public BehaviorStateMachine subStates;
	public virtual void EnterState((Node2D context, Node2D actor) data) {
		var controllable = data.actor.GetNode<Controllable>(typeof(Controllable).Name);
		if (controllable.statePath.LastIndexOf(this) < 0) controllable.statePath.Add(this);
	}
	public virtual void ExitState((Node2D context, Node2D actor) data) {
		var controllable = data.actor.GetNode<Controllable>(typeof(Controllable).Name);
		var path = controllable.statePath;
		var index = path.LastIndexOf(this);

		if (path.Count > index+1)
			path[index + 1].ExitState(data);
		path.RemoveAt(index);
	}
	public virtual void StayState((Node2D context, Node2D actor) data) {
		subStates.UpdateTransitions(data);

		var controllable = data.actor.GetNode<Controllable>(typeof(Controllable).Name);
		var path = controllable.statePath;
		var index = path.LastIndexOf(this);
		if (path.Count > index+1)
			path[index + 1].StayState(data);
	}
}
