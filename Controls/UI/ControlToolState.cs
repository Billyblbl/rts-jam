using Godot;

public class ControlToolState : State {
	public OrderBlueprint order;
	public ControlSystem controls;

	public ControlToolState() {
		GD.Print(string.Format("{0}:{1}", nameof(ControlToolState), GetType().Name));
	}

	public override void _Ready() {
		if (order != null) Input.SetCustomMouseCursor(order.cursor, hotspot: order.hotspot);
	}

	public override void Exitstate() {
		GD.Print(string.Format("{0}:{1}", nameof(Exitstate), GetType().Name));
		Input.SetCustomMouseCursor(null);
		// Input.SetDefaultCursorShape();
		base.Exitstate();
	}

}
