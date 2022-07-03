using Godot;

public class Weapon : Node2D {
	[Export] public float attackDelay = 0f;
	[Export] public Vector2 attackSpawn = Vector2.Zero;
	[Export] public float attackRotation = 0f;
	[Export] public float attackRange = 500f;
	[Export] public PackedScene attack;
	[Signal] public delegate void OnAttack();
	public bool attacking = false;
	float lastAttack = 0;
	[Export] public bool enableDebugTrigger = false;

	ulong start;
	public override void _Ready() {
		base._Ready();
		start = OS.GetSystemTimeMsecs();
	}

	public override void _Process(float delta) {
		base._Process(delta);
		var now = (float)(OS.GetSystemTimeMsecs() - start) / 1000f;

		// GD.Print(string.Format("attacking :{0}, last attack {1}, now {2}, time till next attack {3}", attacking, lastAttack, now, lastAttack + attackDelay - now));
		if (attacking && now - lastAttack > attackDelay) {
			Attack(now);
		}
	}

	public void Attack(float now) {
		EmitSignal(nameof(OnAttack));
		lastAttack = now;
		var newAttack = attack.InstanceOrNull<Attack>();
		newAttack.GlobalRotationDegrees = RotationDegrees + attackRotation;
		newAttack.GlobalPosition = ToGlobal(attackSpawn);
		GetTree().Root.AddChild(newAttack);
		newAttack.Launch();
	}

		//Debug
	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (!enableDebugTrigger) return;
		if (@event is InputEventKey keyEvent && keyEvent.Scancode == (uint)KeyList.Space && keyEvent.Pressed) {
			RotationDegrees = OS.GetSystemTimeMsecs() % 360;
			Attack(0);
		}
	}

}
