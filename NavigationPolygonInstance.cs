using Godot;
using System.Linq;

public class NavigationPolygonInstance : Godot.NavigationPolygonInstance {

	[Export] Color[] colors = new Color[] {Colors.Red};

	public override void _Draw() {
		base._Draw();
		for (int i = 0; i < Navpoly.GetPolygonCount(); i++) {
			var poly = Navpoly.GetPolygon(i);
			var points = poly.Select(index => Navpoly.Vertices[index]).ToArray();
			DrawPolygon(points, colors);
		}
	}
}
