using Godot;

public class CameraController : Camera2D {

	[Export] public string zoomControl;
	[Export] public float sensitivity = 1;
	[Export] public float spring = 1;
	[Export] public float minZoom = 0.1f;
	[Export] public float maxZoom = float.MaxValue;

	[Export] float targetFactor = 1;
	[Export] Vector2 targetPosition;

	public override void _Ready() {
		base._Ready();
		targetPosition = GlobalPosition;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsAction(zoomControl) && @event is InputEventMouseButton emb) {
			targetFactor = Mathf.Clamp(targetFactor + emb.Factor * (emb.ButtonIndex == (int)ButtonList.WheelUp ? sensitivity : -sensitivity), minZoom, maxZoom);
			var anchor = GetGlobalMousePosition();
			var keep = Zoom;
			Zoom = Vector2.One * targetFactor;
			var offsetAnchor = GetGlobalMousePosition();
			var correction = anchor - offsetAnchor;
			targetPosition = GlobalPosition + correction;
			Zoom = keep;
		}
	}

	public override void _Process(float delta) {
		base._Process(delta);
		Zoom = Zoom.LinearInterpolate(Vector2.One * targetFactor, delta * spring);
		GlobalPosition = GlobalPosition.LinearInterpolate(targetPosition, delta * spring);
	}


}
