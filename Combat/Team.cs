using Godot;
using System.Collections.Generic;

public class Team : Resource {

	[Export] public Gauge metal;
	[Export] public Gauge energy;
	[Export(PropertyHint.Layers2dPhysics)] public uint layer;
	public List<Controllable> army = new List<Controllable>();

	public void ActivateStorageAutoExpand() {
		metal.OnOverflow = (value) => metal.max = value;
		energy.OnOverflow = (value) => energy.max = value;
	}

}
