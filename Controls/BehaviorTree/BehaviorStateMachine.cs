using Godot;
using System.Linq;

public struct BehaviorStateMachine {

	public struct Connection {
		public string name;
		public BehaviorState from;
		public BehaviorState to;
		public System.Func<(Node2D, Node2D), bool>[] conditions;

		public Connection(
			string name,
			BehaviorState from,
			BehaviorState to,
			System.Func<(Node2D, Node2D), bool>[] conditions
		) {
			this.name = name;
			this.from = from;
			this.to = to;
			this.conditions = conditions;
		}

		public Connection(
			string name,
			BehaviorState from,
			BehaviorState to,
			System.Func<(Node2D, Node2D), bool> condition
		) {
			this.name = name;
			this.from = from;
			this.to = to;
			this.conditions = new System.Func<(Node2D, Node2D), bool>[] { condition };
		}
	}

	public Connection[] connections;

	public void Transition(BehaviorState exiting, BehaviorState entering, (Node2D context, Node2D actor) data) {
		exiting.ExitState(data);
		entering.EnterState(data);
	}

	public void UpdateTransitions((Node2D context, Node2D actor) data) {
		var controllable = data.actor.GetNode<Controllable>(typeof(Controllable).Name);
		var path = controllable.statePath;

		foreach (var connection in connections ?? new Connection[0]) {
			if (path.Contains(connection.from) && connection.conditions.All((cond) => cond(data))) {
				Transition(connection.from, connection.to, data);
			}
		}
	}

}
