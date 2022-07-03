
using Godot;

public class BehaviorTreeConfig : Node {

	public override void _Ready() {
		Trees.Move = GetNodeOrNull<BehaviorState>(Move ?? "");
		Trees.Follow = GetNodeOrNull<BehaviorState>(Follow  ?? "");
		Trees.TargetedFire = GetNodeOrNull<BehaviorState>(TargetedFire  ?? "");
		Trees.RepairTask = GetNodeOrNull<BehaviorState>(RepairTask  ?? "");
		Trees.UseRepairTool = GetNodeOrNull<BehaviorState>(UseRepairTool  ?? "");
		Trees.Attacking = GetNodeOrNull<BehaviorState>(Attacking  ?? "");
	}

	[Export] public NodePath Move;
	[Export] public NodePath Follow;
	[Export] public NodePath Attacking;
	[Export] public NodePath TargetedFire;
	[Export] public NodePath RepairTask;
	[Export] public NodePath UseRepairTool;

	public static class Trees {
		public static BehaviorState Move;
		public static BehaviorState Follow;
		public static BehaviorState Attacking;
		public static BehaviorState TargetedFire;
		public static BehaviorState RepairTask;
		public static BehaviorState UseRepairTool;
	}

}
