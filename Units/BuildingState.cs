using Godot;

public class BuildingState : Targetting<BuildingState.DoBuild> {
	public BuildingState(Node2D actor, Node2D target) : base(actor, target) {}

	public class DoBuild : BehaviorState {
		public DoBuild(Node2D actor, Node2D target) : base(actor) {
			var buildingTool = actor.GetNode<BuildingTool>(nameof(BuildingTool));
			buildingTool.target = target.GetNode<Health>(nameof(Health));
			buildingTool.SetPhysicsProcess(true);
		}

		public override void Exitstate() {
			var buildingTool = actor.GetNode<BuildingTool>(nameof(BuildingTool));
			buildingTool.SetPhysicsProcess(false);
			buildingTool.target = null;
			base.Exitstate();
		}
	}

	public override void OnTargetInRange(Node2D target) => TransitionTo(() => new DoBuild(actor, target));
	public override bool TargetInRange(Node2D target) {
		var buildingTool = actor.GetNode<BuildingTool>(nameof(BuildingTool));
		return buildingTool.InRange(target as PhysicsBody2D);
	}

}
