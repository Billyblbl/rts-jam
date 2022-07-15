using Godot;

public class Move : BehaviorState {

	public Move(Node2D actor, Vector2 position) : base(actor) {
		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
		pathfind.targetDestination = position;
	}

	public override void UpdateState<T>(T context) {
		base.UpdateState(context);
		var anim = actor.GetNodeOrNull<DirectionalAnimatedSprite>(nameof(DirectionalAnimatedSprite));

		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));

		if (anim != null) {
			anim.direction = pathfind.velocity.Normalized();
			anim.state = this;
		}
	}

	public override void Exitstate() {
		var pathfind = actor.GetNode<PathFindAgent>(nameof(PathFindAgent));
		pathfind.StopPathing();

		var anim = actor.GetNodeOrNull<DirectionalAnimatedSprite>(nameof(DirectionalAnimatedSprite));
		if (anim != null) {
			anim.state = null;
		}

		base.Exitstate();
	}

}
