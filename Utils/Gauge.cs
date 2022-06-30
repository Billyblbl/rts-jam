using Godot;

// have to make this a scene local ressource cause Godot can't export unknown structs/classes
public class Gauge : Resource {
	[Export] public float min;
	[Export] public float max;
	[Export] private float _current;
	public float current {
		get => _current; set {
			_current = Mathf.Clamp(value, min, max);
			if (value < min) OnUnderflow?.Invoke(value);
			else if (value > max) OnOverflow?.Invoke(value);
			OnValueChange?.Invoke(current);
			OnFractionChange?.Invoke(Mathf.InverseLerp(min, max, current));
		}
	}
	public System.Action<float> OnOverflow;
	public System.Action<float> OnUnderflow;
	public System.Action<float> OnFractionChange;
	public System.Action<float> OnValueChange;
}
