using Godot;

public class BuildingTool : Area2D {

	[Export] public float repairSpeed;
	[Export] public float metalConsumption;
	[Export] public float energyConsumption;
	[Export] public NodePath controllable;

	public Health target;
	public Controllable controllableNode;

	public override void _Ready() {
		base._Ready();
		controllableNode = GetNode<Controllable>(controllable);
		SetPhysicsProcess(false);
	}

	public bool InRange(PhysicsBody2D body) => OverlapsBody(body);

	public override void _PhysicsProcess(float delta) {
		base._PhysicsProcess(delta);

		if (InRange(target.GetParent<PhysicsBody2D>())) {
			target.value.current += repairSpeed * delta;
			controllableNode.team.metal.current -= metalConsumption * delta;
			controllableNode.team.energy.current -= energyConsumption * delta;
		}
	}

}
