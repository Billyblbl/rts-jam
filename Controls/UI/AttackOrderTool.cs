using Godot;
using System.Linq;
using System.Collections.Generic;

public class AttackOrderTool : ControlToolState {

	public override void _Input(InputEvent @event) {
		if (@event.IsAction(controls.orderToolAction)) {

			GetTree().SetInputAsHandled();

			if (controls.hovered.Length == 1) {
				controls.GiveOrderToSelection(new AttackOrder(controls.hovered.Single().bodyNode));
			} else {
				//TODO Make an actual attack move order (this one will never end)
				controls.GiveOrderToSelection(new PatrolOrder(new List<Vector2> { controls.GetGlobalMousePosition() }));
			}

			if (!controls.additiveOrder) Exitstate();
		} else if (@event.IsAction(controls.orderToolCancel)) {
			GetTree().SetInputAsHandled();
			Exitstate();
		}
	}


}
