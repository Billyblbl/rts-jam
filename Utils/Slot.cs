using Godot;

public class Slot<T> : Resource {
	T _instance = default;
	public T instance { get => _instance; set {
		_instance = value;
		EmitChanged();
	}}
}
