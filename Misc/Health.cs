using Godot;

public class Health : Node {
	[Export] public Gauge value;
	[Export] public bool enableDebugTrigger = false;
	[Signal] public delegate void OnDepleted();
	[Signal] public delegate void OnChange(float value);

	public override void _Ready() {
		value.OnFractionChange = (value) => EmitSignal(nameof(OnChange), value);
		value.OnUnderflow = (_) => EmitSignal(nameof(OnDepleted));
	}

	//Debug
	public override void _Input(InputEvent @event) {
		base._Input(@event);

		if (!enableDebugTrigger) return;
		if (@event is InputEventKey keyEvent && keyEvent.Scancode == (uint)KeyList.Space) {
			value.current -= 1000;
		}
	}

}
