using Godot;
using System.Collections.Generic;
using System.Linq;

public class StateLabel : Label {

	[Export] NodePath controlablePath;
	Controllable controllable;

	public override void _Ready() {
		base._Ready();
		controllable = GetNode<Controllable>(controlablePath);
	}

	public override void _Process(float delta) {
		base._Process(delta);

		var state = controllable.currentBehavior;
		var names = new List<string>();

		while (state != null) {
			names.Add(state.Name);
			state = state.GetCurrentSubState<BehaviorState>();
		}

		Text = names.Aggregate("", (a,b) => string.Format("{0}.{1}", a, b));
	}

}
