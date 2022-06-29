using Godot;

public class TestOrder : Order {

	public TestOrder(Vector2 position, Node scope, float printDelay = .1f) : base(scope) {
		GlobalPosition = position;
		updateDelay = printDelay;
		Name = GetType().FullName;
	}

	public override void Start(Node data) {
		GD.Print(string.Format("Starting Order {0} on node {1}", Name, data.Name));
	}


	public override void Stop(Node data) {
		GD.Print(string.Format("Stopping Order {0} on node {1}", Name, data.Name));
	}

	float lastUpdate;
	[Export] public float updateDelay;

	float timeNow { get => (float)OS.GetSystemTimeMsecs() / 1000f; }
	public override bool Update(Node data) {
		if (timeNow - lastUpdate > updateDelay) {
			lastUpdate = timeNow;
			GD.Print(string.Format("Updating Order {0} on node {1}", Name, data.Name));
		}
		return false;
	}
}
