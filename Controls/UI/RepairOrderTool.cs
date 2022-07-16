using Godot;
using System.Linq;

public class RepairOrderTool : ControlToolState {

	public override void _Input(InputEvent @event) {
		if (@event.IsAction(controls.orderToolAction)) {

			GetTree().SetInputAsHandled();

			if (controls.hovered.Length == 1) {
				controls.GiveOrderToSelection(new RepairOrder(controls.hovered.Single().bodyNode));
			} /*else {
				controls.GiveOrderToSelection(new PatrolOrder(new List<Vector2> { controls.GetGlobalMousePosition() }));
			}*/

			if (!controls.additiveOrder) Exitstate();
		} else if (@event.IsAction(controls.orderToolCancel)) {
			GetTree().SetInputAsHandled();
			Exitstate();
		}
	}

}
