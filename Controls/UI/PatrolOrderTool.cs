using Godot;
using System.Collections.Generic;
using System.Linq;

public class PatrolOrderTool : ControlToolState {
	public override void _Input(InputEvent @event) {
		if (@event.IsAction(controls.orderToolAction)) {
			if (controls.selection.Length == 0) return;
			GetTree().SetInputAsHandled();

			if (controls.additiveOrder && controls.selection[0].currentOrder is PatrolOrder patrol && controls.selection.All(c => c.currentOrder == patrol)) {
				patrol.waypoints.Add(controls.GetGlobalMousePosition());
			} else {
				controls.GiveOrderToSelection(new PatrolOrder(new List<Vector2> { controls.GetGlobalMousePosition() }));
			}

			if (!controls.additiveOrder) Exitstate();
		} else if (@event.IsAction(controls.orderToolCancel)) {
			GetTree().SetInputAsHandled();
			Exitstate();
		}
	}

}
