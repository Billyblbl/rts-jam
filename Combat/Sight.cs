using Godot;

using System.Collections.Generic;

public class Sight : Area2D {

	public List<Controllable> EnemyInSight = new List<Controllable>();

	public override void _Ready() {
		base._Ready();
		Connect("body_entered", this, nameof(OnBodyEntered));
		Connect("body_exited", this, nameof(OnBodyExited));
	}

	public void OnBodyEntered(Node node) {
		var controllable = node.GetParentOrNull<Controllable>();
		if (controllable != null) EnemyInSight.Add(controllable);
	}

	public void OnBodyExited(Node node) {
		var controllable = node.GetParentOrNull<Controllable>();
		if (controllable != null && EnemyInSight.Contains(controllable)) EnemyInSight.Remove(controllable);
	}

}
