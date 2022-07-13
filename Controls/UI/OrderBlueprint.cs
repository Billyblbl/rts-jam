using Godot;

public class OrderBlueprint : Resource {
	[Export] public Texture icon;
	[Export] public Texture cursor;
	[Export] Vector2 iconHotspot = Vector2.One / 2f;
	[Export] PackedScene instance;
	public Vector2 hotspot { get => new Vector2(cursor.GetSize().x * iconHotspot.x, cursor.GetSize().y * iconHotspot.y); }
	public T Instantiate<T>(ControlSystem controls) where T : ControlToolState{
		var tool = instance.Instance() as T;
		tool.controls = controls;
		tool.order = this;
		controls.AddChild(tool);
		return tool;
	}
}
