using Godot;
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
		Text = controllable.statePath.Select(state => state.Name).Aggregate("State", (a,b) => string.Format("{0}.{1}", a, b));
	}

}
