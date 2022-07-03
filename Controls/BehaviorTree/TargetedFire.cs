using Godot;

public class TargetedFire : BehaviorState {

	public override void EnterState((Node2D context, Node2D actor) data) {
		base.EnterState(data);
		var weapon = data.actor.GetNode<Weapon>(typeof(Weapon).Name);
		weapon.LookAt(data.context.GlobalPosition);
		weapon.attacking = true;
		// GD.Print(string.Format("TargetedFire context = {0}", data.context.Name));
	}

	public override void ExitState((Node2D context, Node2D actor) data) {
		base.ExitState(data);
		var weapon = data.actor.GetNode<Weapon>(typeof(Weapon).Name);
		weapon.attacking = false;
	}

	public override void StayState((Node2D context, Node2D actor) data) {
		base.StayState(data);
		var weapon = data.actor.GetNode<Weapon>(typeof(Weapon).Name);
		// GD.Print(string.Format("TargetedFire context = {0}", data.context.Name));
		weapon.LookAt(data.context.GlobalPosition);
	}

}
